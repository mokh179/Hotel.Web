using Hotel.Application.DTOs.UserProfileDto;
using Hotel.Application.Interfaces.Services.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Hotel.Web.Areas.User.Pages.Profile
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserService _userService;

        public ChangePasswordModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public ChangePasswordDTO PasswordModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _userService.ChangePasswordAsync(userId, PasswordModel);

                TempData["Success"] = "Password updated successfully!";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
