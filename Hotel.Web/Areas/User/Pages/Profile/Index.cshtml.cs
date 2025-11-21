using Hotel.Application.DTOs.UserProfileDTO;
using Hotel.Application.Interfaces.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Hotel.Web.Areas.User.Pages.Profile
{

    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UserDTO Profile { get; set; }
        public bool IsEditing { get; set; }

        public async Task OnGet(bool edit = false)
        {
            IsEditing = edit;
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Profile = await _userService.GetProfileAsync(userId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                Profile.Id= userId;
                await _userService.UpdateProfileAsync(Profile);
                TempData["Success"] = "Profile updated successfully!";
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
