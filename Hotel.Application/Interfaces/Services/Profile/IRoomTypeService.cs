using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services.Profile
{
    public interface IRoomTypeService
    {
        Task<List<RoomTypeDTO>> GetAllAsync();
        Task<RoomTypeDTO?> GetByIdAsync(Guid id);
        Task<RoomTypeDTO> CreateAsync(CreateRoomTypeDTO dto);
        Task<RoomTypeDTO?> UpdateAsync(UpdateRoomTypeDTO dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
