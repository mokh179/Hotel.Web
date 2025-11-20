using Hotel.Application.DTOs.AuthDtos;
using Hotel.Application.Interfaces.Services.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Account.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;


        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public LoginDto Input { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _authService.LoginAsync(Input);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return Page();
            }

            return RedirectToPage("/Dashboard", new { area = "User" });
        }
    }
}
