using Hotel.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces
{
    public interface IRoomService
    {
        // Create
        Task AddRoomAsync(RoomDTO roomDto);

        // Read
        Task<IEnumerable<RoomDTO>> GetRoomsByHotelAsync(int hotelId);
        Task<RoomDTO> GetRoomByIdAsync(int roomId);

        // Update
        Task UpdateRoomAsync(int roomId, RoomDTO roomDto);

        // Delete
        Task DeleteRoomAsync(int roomId);

        Task AddRoomsAsync(IEnumerable<RoomDTO> roomDtos);
        Task UpdateRoomsAsync(IEnumerable<RoomDTO> roomDtos);
        Task DeleteRoomsAsync(IEnumerable<int> roomIds);
    }
}
