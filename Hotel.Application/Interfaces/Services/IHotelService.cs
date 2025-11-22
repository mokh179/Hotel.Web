using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services
{
    public interface IHotelService
    {
        Task<HotelDTO> GetByIdAsync(Guid id);
        Task<List<HotelDTO>> GetAllAsync();
        Task<HotelDTO> CreateAsync(CreateHotelDTO dto);
        Task<HotelDTO> UpdateAsync(UpdateHotelDTO dto);
        Task<bool> DeleteAsync(Guid id);

        // Bulk operations
        Task AddRangeAsync(IEnumerable<CreateHotelDTO> dtos);
        Task SoftDeleteRangeAsync(IEnumerable<Guid> ids);
        Task<List<HotelDTO>> GetAllCachedAsync();
        Task<PagedResult<HotelDTO>> SearchHotelsAsync(
                                                        string name,
                                                        Guid? countryId,
                                                        Guid? cityId,
                                                        int page,
                                                        int pageSize);
    }
}
