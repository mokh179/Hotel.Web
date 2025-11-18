using Hotel.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces
{
    public interface IHotelService
    {
        Task AddHotelAsync(HotelDTO hotelDto);

        // Read
        Task<IEnumerable<HotelDTO>> GetAllHotelsAsync();
        Task<HotelDTO> GetHotelByIdAsync(int hotelId);

        // Update
        Task UpdateHotelAsync(int hotelId, HotelDTO hotelDto);

        // Delete
        Task DeleteHotelAsync(int hotelId);

        Task AddHotelsAsync(IEnumerable<HotelDTO> hotelDtos);
        Task UpdateHotelsAsync(IEnumerable<HotelDTO> hotelDtos);
        Task DeleteHotelsAsync(IEnumerable<int> hotelIds);
    }
}
