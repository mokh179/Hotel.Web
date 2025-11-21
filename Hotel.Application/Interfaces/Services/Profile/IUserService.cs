using Hotel.Application.DTOs.UserProfileDTO;
using Hotel.Application.DTOs.UserProfileDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services.Profile
{
    public interface IUserService
    {
        Task<UserDTO> GetProfileAsync(Guid userId);
        Task UpdateProfileAsync(UserDTO dto);
        Task ChangePasswordAsync(Guid userId, ChangePasswordDTO dto);

        Task<List<UserDTO>> GetAllAsync();

    }
}
