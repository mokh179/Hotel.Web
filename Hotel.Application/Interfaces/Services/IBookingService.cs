using Hotel.Application.DTOs.BookingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<BookingDTO> GetByIdAsync(Guid id);
        Task<List<BookingDTO>> GetAllAsync();
        Task<BookingDTO> CreateAsync(CreateBookingDTO dto);
        Task<BookingDTO> UpdateAsync(UpdateBookingDTO dto);
        Task DeleteAsync(Guid id);

        Task<List<BookingDTO>> GetByUserIdAsync(Guid userId);

        // Bulk operations
        Task AddRangeAsync(IEnumerable<CreateBookingDTO> dtos);
        Task SoftDeleteRangeAsync(IEnumerable<Guid> ids);

        Task<int> CountActiveBookingsAsync(Guid userId);
        Task<int> CountPastBookingsAsync(Guid userId);
    }
}
