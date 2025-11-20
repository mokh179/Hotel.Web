using Hotel.Application.DTOs.AuthDtos;
using Hotel.Application.Interfaces.Services.Authentication;
using Hotel.Infrastructure.Identity;
using Hotel.Web.Areas.Account.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Web.Areas.Account.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthService _authService;


        public RegisterModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public RegisterDto Input { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var result = await _authService.RegisterAsync(Input);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                return Page();
            }

            // ?? ??? ??????? ? ??? ?????? ????????
            return RedirectToPage("/Index", new { area = "" });
        }

    }
}
