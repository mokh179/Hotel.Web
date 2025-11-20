using AutoMapper;
using Hotel.Application.DTOs.BookingDto;
using Hotel.Application.Interfaces;
using Hotel.Application.Interfaces.Services;
using Hotel.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Services
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

        public async Task<BookingDTO> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Bookings
                .Query()
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .FirstOrDefaultAsync(b => b.Id == id);

            return _mapper.Map<BookingDTO>(entity);
        }

        // ===============================
        // GET ALL BOOKINGS
        // ===============================
        public async Task<List<BookingDTO>> GetAllAsync()
        {
            var entities = await _unitOfWork.Bookings
                .Query()
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .ToListAsync();

            return _mapper.Map<List<BookingDTO>>(entities);
        }

    
        public async Task<BookingDTO> CreateAsync(CreateBookingDTO dto)
        {
            // PricePerNight يجي من الـ Room مش من الـ DTO
            var room = await _unitOfWork.Rooms.GetByIdAsync(dto.RoomId);
            if (room == null)
                throw new Exception("Room not found.");

            var entity = new Booking(dto.RoomId, dto.UserId, dto.DateRange.From, dto.DateRange.To);

            await _unitOfWork.Bookings.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            // لازم نعمل Include علشان نرجع Room + Hotel
            entity = await _unitOfWork.Bookings
                .Query()
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .FirstAsync(b => b.Id == entity.Id);

            return _mapper.Map<BookingDTO>(entity);
        }

        
        public async Task<BookingDTO> UpdateAsync(UpdateBookingDTO dto)
        {
            var entity = await _unitOfWork.Bookings.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new Exception("Booking not found");

            entity.RoomId = dto.RoomId;
            entity.CheckIn = dto.DateRange.From;
            entity.CheckOut = dto.DateRange.To;

            _unitOfWork.Bookings.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            entity = await _unitOfWork.Bookings
                .Query()
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .FirstOrDefaultAsync(b => b.Id == entity.Id);

            return _mapper.Map<BookingDTO>(entity);
        }

        
        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Bookings.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        
        public async Task<List<BookingDTO>> GetByUserIdAsync(Guid userId)
        {
            var entities = await _unitOfWork.Bookings
                .Query()
                .Include(b => b.Room)
                    .ThenInclude(r => r.Hotel)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<BookingDTO>>(entities);
        }

        
        public async Task AddRangeAsync(IEnumerable<CreateBookingDTO> dtos)
        {
            var entities = dtos.Select(d => new Booking(d.RoomId, d.UserId, d.DateRange.To,d.DateRange.From)).ToList();

            await _unitOfWork.Bookings.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SoftDeleteRangeAsync(IEnumerable<Guid> ids)
        {
            await _unitOfWork.Bookings.SoftDeleteRangeAsync(ids);
            await _unitOfWork.SaveChangesAsync();
        }

        
        public async Task<int> CountActiveBookingsAsync(Guid userId)
        {
            var now = DateTime.UtcNow;

            return await _unitOfWork.Bookings
                .Query()
                .Where(b => b.UserId == userId && b.CheckOut >= now)
                .CountAsync();
        }

        public async Task<int> CountPastBookingsAsync(Guid userId)
        {
            var now = DateTime.UtcNow;

            return await _unitOfWork.Bookings
                .Query()
                .Where(b => b.UserId == userId && b.CheckOut < now)
                .CountAsync();
        }
        public async Task<bool> IsRoomAvailable(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            return !await _unitOfWork.Bookings
                .Query()
                .Where(b => b.RoomId == roomId)
                .Where(b => b.CheckIn < checkOut && checkIn < b.CheckOut)
                .AnyAsync();
        }
    }
}
