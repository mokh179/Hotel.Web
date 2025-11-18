using Hotel.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces
{
    public interface IBookingService
    {
        // Create
        Task CreateBookingAsync(BookingDTO bookingDto);

        // Read
        Task<IEnumerable<BookingDTO>> GetAllBookingsAsync();
        Task<BookingDTO> GetBookingByIdAsync(int bookingId);

        // Update
        Task UpdateBookingAsync(int bookingId, BookingDTO bookingDto);

        // Delete
        Task DeleteBookingAsync(int bookingId);

    }
}
