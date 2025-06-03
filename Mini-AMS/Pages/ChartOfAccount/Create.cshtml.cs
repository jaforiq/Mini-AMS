using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mini_AMS.Models;
using Mini_AMS.Services;
using System.ComponentModel.DataAnnotations;

namespace Mini_AMS.Pages.ChartOfAccount
{
    [Authorize(Roles = "Admin,Accountant")]
    public class CreateModel : PageModel
    {
        private readonly ChartOfAccountService _accountService;

        public CreateModel(ChartOfAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<SelectListItem> ParentAccounts { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string Name { get; set; }
            public int? ParentId { get; set; }
            [Required]
            public string Type { get; set; }
        }

        public async Task OnGetAsync()
        {
            // Load parent accounts for dropdown
            ParentAccounts = await GetParentAccountsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ParentAccounts = await GetParentAccountsAsync();
                return Page();
            }

            await _accountService.CreateAccountAsync(Input.Name, Input.ParentId, Input.Type);
            return RedirectToPage("/Index");
        }

        private async Task<List<SelectListItem>> GetParentAccountsAsync()
        {
            // You need to implement a method in ChartOfAccountService to get all accounts
            var accounts = await _accountService.GetAllAccountsAsync();
            return accounts.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();
        }
    }
}
