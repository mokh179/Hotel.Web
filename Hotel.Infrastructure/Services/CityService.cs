using AutoMapper;
using Hotel.Application.DTOs.Locations.City;
using Hotel.Application.Interfaces.Services;
using Hotel.Application.Interfaces;
using Hotel.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Hotel.Infrastructure.Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        private const string CACHE_ALL = "cities_all";
        private const string CACHE_ONE = "city_";

        public CityService(IUnitOfWork uow, IMapper mapper, IMemoryCache cache)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<CityDTO>> GetAllAsync()
        {
            if (_cache.TryGetValue(CACHE_ALL, out List<CityDTO>? cached))
                return cached;

            var list = await _uow.Cities.Query()
                        .Include(c => c.Country)
                        .ToListAsync();

            var dto = _mapper.Map<List<CityDTO>>(list);

            _cache.Set(CACHE_ALL, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        public async Task<List<CityDTO>> GetByCountryAsync(Guid countryId)
        {
            var all = await GetAllAsync();
            return all.Where(c => c.CountryId == countryId).ToList();
        }

        public async Task<CityDTO?> GetByIdAsync(Guid id)
        {
            string key = CACHE_ONE + id;

            if (_cache.TryGetValue(key, out CityDTO? cached))
                return cached;

                var entity = await _uow.Cities
           .Query()
           .Include(c => c.Country)
           .FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null)
                return null;

            var dto = _mapper.Map<CityDTO>(entity);

            _cache.Set(key, dto, TimeSpan.FromMinutes(10));
            return dto;
        }

        public async Task<CityDTO> CreateAsync(CreateCityDTO dto)
        {
            bool exists = await _uow.Cities.ExistsAsync(c =>
                c.Name.ToLower().Trim() == dto.Name.ToLower().Trim()
                && c.CountryId == dto.CountryId
            );

            if (exists)
                throw new Exception("City already exists in this country.");

            var entity = _mapper.Map<City>(dto);

            await _uow.Cities.AddAsync(entity);
            await _uow.SaveChangesAsync();

            _cache.Remove(CACHE_ALL);

            return _mapper.Map<CityDTO>(entity);
        }

        public async Task<CityDTO?> UpdateAsync(UpdateCityDTO dto)
        {
            var entity = await _uow.Cities.GetByIdAsync(dto.Id);
            if (entity == null) return null;

            bool exists = await _uow.Cities.ExistsAsync(c =>
                c.Name.ToLower().Trim() == dto.Name.ToLower().Trim()
                && c.CountryId == dto.CountryId
                && c.Id != dto.Id
            );

            if (exists)
                throw new Exception("Another city with the same name already exists in this country.");

            _mapper.Map(dto, entity);
            _uow.Cities.Update(entity);

            await _uow.SaveChangesAsync();

            _cache.Remove(CACHE_ALL);
            _cache.Remove(CACHE_ONE + dto.Id);

            return _mapper.Map<CityDTO>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var e = await _uow.Cities.GetByIdAsync(id);
            if (e == null) return false;

            await _uow.Cities.DeleteAsync(id);
            await _uow.SaveChangesAsync();

            _cache.Remove(CACHE_ALL);
            _cache.Remove(CACHE_ONE + id);

            return true;
        }
    }

}
