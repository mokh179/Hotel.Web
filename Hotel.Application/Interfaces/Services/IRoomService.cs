using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Pagination;
using Hotel.Application.DTOs.RoomDto;
using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;

namespace Hotel.Application.Interfaces.Services
{
    public interface IRoomService
    {
        Task<RoomDTO> GetByIdAsync(Guid id);
        Task<List<RoomDTO>> GetAllAsync();
        Task<RoomDTO> CreateAsync(CreateRoomDTO dto);
        Task<RoomDTO> UpdateAsync(UpdateRoomDTO dto);
        Task<bool> DeleteAsync(Guid id);

        Task<List<RoomDTO>> GetByHotelIdAsync(Guid hotelId);

        // Search + Pagination
        Task<PagedResult<RoomDTO>> SearchRoomsAsync(
            string? number,
            Guid? hotelId,
            Guid? roomTypeId,
            decimal? minPrice,
            decimal? maxPrice,
            bool? available,
            bool? booked,
            int page,
            int pageSize);

        // Cached lists
        Task<List<RoomTypeDTO>> GetRoomTypesCachedAsync();
        Task<List<HotelDTO>> GetHotelsCachedAsync();

        // bulk
        Task AddRangeAsync(IEnumerable<CreateRoomDTO> dtos);
        Task SoftDeleteRangeAsync(IEnumerable<Guid> ids);
    }
}
