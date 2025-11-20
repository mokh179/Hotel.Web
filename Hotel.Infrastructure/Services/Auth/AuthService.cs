using Hotel.Application.DTOs.AuthDtos;
using Hotel.Application.Interfaces.Services.Authentication;
using Hotel.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AuthResult> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return AuthResult.Fail(result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(user, isPersistent: false);

            return AuthResult.Success(user.Id);
        }

        public async Task<AuthResult> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return AuthResult.Fail(new[] { "Invalid email or password." });

            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, false, false);

            if (!result.Succeeded)
                return AuthResult.Fail(new[] { "Invalid email or password." });

            return AuthResult.Success(user.Id);
        }
    }
}
