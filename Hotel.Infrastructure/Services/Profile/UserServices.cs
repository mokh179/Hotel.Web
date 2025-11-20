using Hotel.Application.DTOs.UserProfileDto;
using Hotel.Application.Interfaces.Services.Profile;
using Hotel.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Services.Profile
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDTO> GetProfileAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            return new UserProfileDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        public async Task UpdateProfileAsync(UserProfileDTO model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
                throw new Exception("User not found");

            // Check email unique
            if (user.Email != model.Email)
            {
                var exists = await _userManager.FindByEmailAsync(model.Email);
                if (exists != null)
                    throw new Exception("This email is already used by another account.");
            }

            // Update fields
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        public async Task ChangePasswordAsync(Guid userId, ChangePasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new Exception("User not found");

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword
            );

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

    }

}
