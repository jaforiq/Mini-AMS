using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mini_AMS.Models;
using Mini_AMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mini_AMS.Pages.Voucher
{
    [Authorize(Roles = "Admin,Accountant")]
    public class JournalModel : PageModel
    {
        private readonly ChartOfAccountService _accountService;
        private readonly VoucherService _voucherService;
        private readonly UserManager<ApplicationUser> _userManager;

        public JournalModel(ChartOfAccountService accountService, VoucherService voucherService, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _voucherService = voucherService;
            _userManager = userManager;
        }

        [BindProperty]
        public VoucherHeader Input { get; set; } = new VoucherHeader
        {
            Date = DateTime.Today,
            Lines = new List<VoucherLine>
            {
                new VoucherLine(),
                //new VoucherLine()
            }
        };

        public List<SelectListItem> AccountList { get; set; } = new();

        public async Task OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            AccountList = users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = $"{u.FirstName} {u.LastName}".Trim()
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            AccountList = accounts.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            //Input.VoucherType = "Journal";
            //await _voucherService.SaveVoucherAsync(Input);
            try
            {
                Input.VoucherType = "Journal";
                await _voucherService.SaveVoucherAsync(Input);
            }
            catch (Exception ex)
            {
                // Log or display ex.Message
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
