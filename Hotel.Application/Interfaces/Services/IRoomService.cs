using Hotel.Application.DTOs.RoomDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task<RoomDTO> GetByIdAsync(Guid id);
        Task<List<RoomDTO>> GetAllAsync();
        Task<RoomDTO> CreateAsync(CreateRoomDTO dto);
        Task<RoomDTO> UpdateAsync(UpdateRoomDTO dto);
        Task DeleteAsync(Guid id);

        Task<List<RoomDTO>> GetByHotelIdAsync(Guid hotelId);

        // Bulk operations
        Task AddRangeAsync(IEnumerable<CreateRoomDTO> dtos);
        Task SoftDeleteRangeAsync(IEnumerable<Guid> ids);
    }
}
