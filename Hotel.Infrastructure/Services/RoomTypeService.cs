using AutoMapper;
using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;
using Hotel.Application.Interfaces.Services.Profile;
using Hotel.Application.Interfaces;
using Hotel.Entities.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Services
{
    class RoomTypeService : IRoomTypeService
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        private const string CACHE_ALL = "roomtypes_all";
        private const string CACHE_ONE = "roomtype_";

        public RoomTypeService(IUnitOfWork unitofwork, IMapper mapper, IMemoryCache cache)
        {
            _unitofwork = unitofwork;
            _mapper = mapper;
            _cache = cache;
        }


        public async Task<List<RoomTypeDTO>> GetAllAsync()
        {
            if (_cache.TryGetValue(CACHE_ALL, out List<RoomTypeDTO>? cached))
                return cached;

            var types = await _unitofwork.RoomTypes.Query().ToListAsync();
            var dto = _mapper.Map<List<RoomTypeDTO>>(types);

            _cache.Set(CACHE_ALL, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

 
        public async Task<RoomTypeDTO?> GetByIdAsync(Guid id)
        {
            string key = CACHE_ONE + id;

            if (_cache.TryGetValue(key, out RoomTypeDTO? cached))
                return cached;

            var entity = await _unitofwork.RoomTypes.GetByIdAsync(id);
            if (entity == null)
                return null;

            var dto = _mapper.Map<RoomTypeDTO>(entity);

            _cache.Set(key, dto, TimeSpan.FromMinutes(10));

            return dto;
        }


        public async Task<RoomTypeDTO> CreateAsync(CreateRoomTypeDTO dto)
        {
            // Validation: prevent duplicate
            bool exists = await _unitofwork.RoomTypes.ExistsAsync(r =>
                r.Name.ToLower().Trim() == dto.Name.ToLower().Trim()
            );

            if (exists)
                throw new Exception("Room type already exists.");

            var entity = _mapper.Map<RoomType>(dto);
            await _unitofwork.RoomTypes.AddAsync(entity);
            await _unitofwork.SaveChangesAsync();

            // invalidate cache
            _cache.Remove(CACHE_ALL);

            return _mapper.Map<RoomTypeDTO>(entity);
        }

        public async Task<RoomTypeDTO?> UpdateAsync(UpdateRoomTypeDTO dto)
        {
            var entity = await _unitofwork.RoomTypes.GetByIdAsync(dto.Id);
            if (entity == null)
                return null;

            // Validation: prevent duplicate
            bool exists = await _unitofwork.RoomTypes.ExistsAsync(r =>
                r.Name.ToLower().Trim() == dto.Name.ToLower().Trim()
                && r.Id != dto.Id
            );

            if (exists)
                throw new Exception("Another room type with the same name already exists.");

            _mapper.Map(dto, entity);
            _unitofwork.RoomTypes.Update(entity);
            await _unitofwork.SaveChangesAsync();

            _cache.Remove(CACHE_ALL);
            _cache.Remove(CACHE_ONE + dto.Id);

            return _mapper.Map<RoomTypeDTO>(entity);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitofwork.RoomTypes.GetByIdAsync(id);
            if (entity == null)
                return false;

            await _unitofwork.RoomTypes.DeleteAsync(id);
            await _unitofwork.SaveChangesAsync();

            _cache.Remove(CACHE_ALL);
            _cache.Remove(CACHE_ONE + id);

            return true;
        }
    }
}
