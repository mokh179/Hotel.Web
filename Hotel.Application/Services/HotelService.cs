using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Application.Interfaces;
using Hotel.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Create
        public async Task AddHotelAsync(HotelDTO hotelDto)
        {
            var hotel = _mapper.Map<Hotel.Entities.Entities.Hotel>(hotelDto);
            await _unitOfWork.Hotels.AddAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
        }

        // Read All
        public async Task<IEnumerable<HotelDTO>> GetAllHotelsAsync()
        {
            var hotels = await _unitOfWork.Hotels.GetAllAsync();
            return _mapper.Map<IEnumerable<HotelDTO>>(hotels);
        }

        // Read by Id
        public async Task<HotelDTO> GetHotelByIdAsync(int hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            return _mapper.Map<HotelDTO>(hotel);
        }

        // Update
        public async Task UpdateHotelAsync(int hotelId, HotelDTO hotelDto)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            if (hotel == null) throw new Exception("Hotel not found");

            _mapper.Map(hotelDto, hotel); // تحديث الحقول
            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.SaveChangesAsync();
        }

        // Delete (Soft Delete)
        public async Task DeleteHotelAsync(int hotelId)
        {
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(hotelId);
            if (hotel == null) throw new Exception("Hotel not found");

            _unitOfWork.Hotels.Delete(hotel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddHotelsAsync(IEnumerable<HotelDTO> hotelDtos)
        {
            var hotels = _mapper.Map<IEnumerable<Hotel.Entities.Entities.Hotel>>(hotelDtos);
            await _unitOfWork.Hotels.AddRangeAsync(hotels);
            await _unitOfWork.SaveChangesAsync();
        }

        // Bulk Update
        public async Task UpdateHotelsAsync(IEnumerable<HotelDTO> hotelDtos)
        {
            foreach (var dto in hotelDtos)
            {
                await _unitOfWork.Hotels.Entities
                    .Where(h => h.Id == dto.Id)
                    .ExecuteUpdateAsync(h => h
                        .SetProperty(p => p.Name, dto.Name)
                        .SetProperty(p => p.Location, dto.Location)
                    );
            }
        }

        // Bulk Delete (Soft Delete)
        public async Task DeleteHotelsAsync(IEnumerable<int> hotelIds)
        {
            await _unitOfWork.Hotels.Entities
                .Where(h => hotelIds.Contains(h.Id))
                .ExecuteUpdateAsync(h => h
                    .SetProperty(p => p.IsDeleted, true)
                    .SetProperty(p => p.DeletedAt, DateTime.UtcNow)
                );
        }
    }
}
