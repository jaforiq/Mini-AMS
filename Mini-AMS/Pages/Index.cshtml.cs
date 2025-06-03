using Mini_AMS.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace Mini_AMS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ChartOfAccountService _accountService;

        public IndexModel(ILogger<IndexModel> logger, ChartOfAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public List<Mini_AMS.Models.ChartOfAccount> Accounts { get; set; } = new();

        public async Task OnGetAsync()
        {
            Accounts = await _accountService.GetAllAccountsAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _accountService.DeleteAccountAsync(id);
            return RedirectToPage();
        }
    }
}
