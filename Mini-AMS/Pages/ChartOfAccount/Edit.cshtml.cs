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
    public class EditModel : PageModel
    {
        private readonly ChartOfAccountService _accountService;

        public EditModel(ChartOfAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<SelectListItem> ParentAccounts { get; set; } = new();

        public class InputModel
        {
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            public int? ParentId { get; set; }
            [Required]
            public string Type { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var account = (await _accountService.GetAllAccountsAsync()).FirstOrDefault(a => a.Id == id);
            if (account == null)
                return NotFound();

            Input = new InputModel
            {
                Id = account.Id,
                Name = account.Name,
                ParentId = account.ParentId,
                Type = account.Type
            };
            ParentAccounts = (await _accountService.GetAllAccountsAsync())
                .Where(a => a.Id != id)
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ParentAccounts = await _accountService.GetAllAccountsAsync()
                    .ContinueWith(t => t.Result.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList());
                return Page();
            }

            await _accountService.UpdateAccountAsync(Input.Id, Input.Name, Input.ParentId, Input.Type);
            return RedirectToPage("/Index");
        }
    }
}
