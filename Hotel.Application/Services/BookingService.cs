using AutoMapper;
using Hotel.Application.DTOs.BookingDto;
using Hotel.Application.Interfaces;
using Hotel.Application.Interfaces.Services;
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

        public async Task<BookingDTO> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Bookings.GetByIdAsync(id);
            return _mapper.Map<BookingDTO>(entity);
        }

        public async Task<List<BookingDTO>> GetAllAsync()
        {
            var entities = await _unitOfWork.Bookings.GetAllAsync();
            return _mapper.Map<List<BookingDTO>>(entities);
        }

        public async Task<BookingDTO> CreateAsync(CreateBookingDTO dto)
        {
            var entity = _mapper.Map<Booking>(dto);
            await _unitOfWork.Bookings.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BookingDTO>(entity);
        }

        public async Task<BookingDTO> UpdateAsync(UpdateBookingDTO dto)
        {
            var entity = await _unitOfWork.Bookings.GetByIdAsync(dto.Id);
            if (entity == null) throw new Exception("Booking not found");

            _mapper.Map(dto, entity);
            _unitOfWork.Bookings.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookingDTO>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Bookings.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<BookingDTO>> GetByUserIdAsync(Guid userId)
        {
            var entities = await _unitOfWork.Bookings.FindAsync(b => b.UserId == userId);
            return _mapper.Map<List<BookingDTO>>(entities);
        }

        public async Task AddRangeAsync(IEnumerable<CreateBookingDTO> dtos)
        {
            var entities = _mapper.Map<IEnumerable<Booking>>(dtos);
            await _unitOfWork.Bookings.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SoftDeleteRangeAsync(IEnumerable<Guid> ids)
        {
            await _unitOfWork.Bookings.SoftDeleteRangeAsync(ids);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
