using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Application.Interfaces;
using Hotel.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Create
        public async Task CreateBookingAsync(BookingDTO bookingDto)
        {
            var booking = _mapper.Map<Booking>(bookingDto);
            await _unitOfWork.Bookings.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();
        }

        // Read All
        public async Task<IEnumerable<BookingDTO>> GetAllBookingsAsync()
        {
            var bookings = await _unitOfWork.Bookings.GetAllAsync();
            return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
        }

        // Read by Id
        public async Task<BookingDTO> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
            return _mapper.Map<BookingDTO>(booking);
        }

        // Update
        public async Task UpdateBookingAsync(int bookingId, BookingDTO bookingDto)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
            if (booking == null) throw new Exception("Booking not found");

            _mapper.Map(bookingDto, booking);
            _unitOfWork.Bookings.Update(booking);
            await _unitOfWork.SaveChangesAsync();
        }

        // Delete (Soft Delete)
        public async Task DeleteBookingAsync(int bookingId)
        {
            var booking = await _unitOfWork.Bookings.GetByIdAsync(bookingId);
            if (booking == null) throw new Exception("Booking not found");

            _unitOfWork.Bookings.Delete(booking);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
