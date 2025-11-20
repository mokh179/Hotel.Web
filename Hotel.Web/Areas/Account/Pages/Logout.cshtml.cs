using Hotel.Application.Interfaces.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Account.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IAuthService _authService;
        public LogoutModel(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _authService.LogoutAsync();
            return RedirectToPage("/Privacy", new { area = "" });
        }
    }
}
