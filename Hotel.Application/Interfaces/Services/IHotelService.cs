using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Pagination;

namespace Hotel.Application.Interfaces.Services
{
    public interface IHotelService
    {
        Task<List<HotelDTO>> GetAllAsync();

        Task<PagedResult<HotelDTO>> SearchAsync(
            string? name,
            Guid? countryId,
            Guid? cityId,
            int page,
            int pageSize);

        Task<HotelDTO?> GetByIdAsync(Guid id);

        Task<HotelDTO> CreateAsync(CreateHotelDTO dto);

        Task<HotelDTO> UpdateAsync(UpdateHotelDTO dto);

        Task<bool> DeleteAsync(Guid id);

        Task AddRangeAsync(IEnumerable<CreateHotelDTO> dtos);

        Task<bool> SoftDeleteRangeAsync(IEnumerable<Guid> ids);
    }
}
