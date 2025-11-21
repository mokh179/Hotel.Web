using AutoMapper;
using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.Interfaces;
using Hotel.Application.Interfaces.Services;
using Hotel.Entities.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Hotel.Infrastructure.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        private const string CACHE_ALL = "countries_all";
        private const string CACHE_ONE = "country_";

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        // ------------------- GET ALL + CACHING -------------------
        public async Task<List<CountryDTO>> GetAllAsync()
        {
            if (_cache.TryGetValue(CACHE_ALL, out List<CountryDTO>? cached))
                return cached!;

            var countries = await _unitOfWork.Countries.GetAllAsync();
            var dto = _mapper.Map<List<CountryDTO>>(countries);

            _cache.Set(CACHE_ALL, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        // ------------------- GET BY ID + CACHING -------------------
        public async Task<CountryDTO?> GetByIdAsync(Guid id)
        {
            string cacheKey = CACHE_ONE + id;

            if (_cache.TryGetValue(cacheKey, out CountryDTO? cached))
                return cached;

            var entity = await _unitOfWork.Countries.GetByIdAsync(id);
            if (entity == null) return null;

            var dto = _mapper.Map<CountryDTO>(entity);

            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

            return dto;
        }

        // ------------------- CREATE + VALIDATION + CACHE INVALIDATION -------------------
        public async Task<CountryDTO> CreateAsync(CreateCountryDTO dto)
        {
            // **Duplicate validation**
            bool exists = await _unitOfWork.Countries
                .ExistsAsync(c => c.Name.ToLower().Trim() == dto.Name.ToLower().Trim());

            if (exists)
                throw new Exception("This country already exists.");

            var entity = _mapper.Map<Country>(dto);

            await _unitOfWork.Countries.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            // Invalidate cache
            _cache.Remove(CACHE_ALL);

            return _mapper.Map<CountryDTO>(entity);
        }

        // ------------------- UPDATE + VALIDATION + CACHE INVALIDATION -------------------
        public async Task<CountryDTO?> UpdateAsync(UpdateCountryDTO dto)
        {
            var entity = await _unitOfWork.Countries.GetByIdAsync(dto.Id);
            if (entity == null) return null;

            // Check duplicate name (excluding itself)
            bool exists = await _unitOfWork.Countries
                .ExistsAsync(c =>
                    c.Name.ToLower().Trim() == dto.Name.ToLower().Trim()
                    && c.Id != dto.Id);

            if (exists)
                throw new Exception("Another country with the same name already exists.");

            _mapper.Map(dto, entity);
            _unitOfWork.Countries.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            // Invalidate cache
            _cache.Remove(CACHE_ALL);
            _cache.Remove(CACHE_ONE + dto.Id);

            return _mapper.Map<CountryDTO>(entity);
        }

        // ------------------- DELETE + CACHE INVALIDATION -------------------
        public async Task<bool> DeleteAsync(Guid id)
        {
            var e = await _unitOfWork.Countries.GetByIdAsync(id);
            if (e == null) return false;

            await _unitOfWork.Countries.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            _cache.Remove(CACHE_ALL);
            _cache.Remove(CACHE_ONE + id);

            return true;
        }
    }
}
