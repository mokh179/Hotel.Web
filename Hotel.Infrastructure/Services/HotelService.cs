using AutoMapper;
using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.Interfaces;
using Hotel.Application.Interfaces.Services;
using Hotel.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Services
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

        public async Task<HotelDTO> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Hotels.Query()
                                .Include(h => h.City)
                                .ThenInclude(c => c.Country)
                                .FirstOrDefaultAsync(h => h.Id == id);
            return _mapper.Map<HotelDTO>(entity);
        }

        public async Task<List<HotelDTO>> GetAllAsync()
        {
            var entities = await _unitOfWork.Hotels.Query()
                                    .Include(h => h.City)
                                    .ThenInclude(c => c.Country)
                                    .ToListAsync();
            return _mapper.Map<List<HotelDTO>>(entities);
        }

        public async Task<HotelDTO> CreateAsync(CreateHotelDTO dto)
        {
            var entity = _mapper.Map<Hotel.Entities.Entities.Hotel>(dto);
            await _unitOfWork.Hotels.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<HotelDTO>(entity);
        }

        public async Task<HotelDTO> UpdateAsync(UpdateHotelDTO dto)
        {
            var entity = await _unitOfWork.Hotels.GetByIdAsync(dto.Id);
            if (entity == null) throw new Exception("Hotel not found");

            _mapper.Map(dto, entity);
            _unitOfWork.Hotels.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<HotelDTO>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Hotels.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<CreateHotelDTO> dtos)
        {
            var entities = _mapper.Map<IEnumerable<Hotel.Entities.Entities.Hotel>>(dtos);
            await _unitOfWork.Hotels.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SoftDeleteRangeAsync(IEnumerable<Guid> ids)
        {
            await _unitOfWork.Hotels.SoftDeleteRangeAsync(ids);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
