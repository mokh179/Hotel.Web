using Hotel.Application.DTOs.UserProfileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services.Profile
{
    public interface IUserService
    {
        Task<UserProfileDTO> GetProfileAsync(Guid userId);
        Task UpdateProfileAsync(UserProfileDTO dto);
        Task ChangePasswordAsync(Guid userId, ChangePasswordDTO dto);

    }
}
