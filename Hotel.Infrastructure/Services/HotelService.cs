using AutoMapper;
using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Pagination;
using Hotel.Application.Interfaces;
using Hotel.Application.Interfaces.Services;
using Hotel.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Hotel.Infrastructure.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HotelService> _logger;

        private const string CACHE_LIST = "hotels_all";
        private const string CACHE_ITEM = "hotel_";

        public HotelService(
            IUnitOfWork uow,
            IMapper mapper,
            IMemoryCache cache,
            ILogger<HotelService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        // ----------------------------------------------
        // GET ALL (Cached)
        // ----------------------------------------------
        public async Task<List<HotelDTO>> GetAllAsync()
        {
            try
            {
                if (_cache.TryGetValue(CACHE_LIST, out List<HotelDTO>? cached))
                    return cached!;

                var entities = await _uow.Hotels
                    .Query()
                    .Include(h => h.City)
                    .ThenInclude(c => c.Country)
                    .Include(h => h.Rooms)
                    .ToListAsync();

                var dto = _mapper.Map<List<HotelDTO>>(entities);

                _cache.Set(CACHE_LIST, dto, TimeSpan.FromMinutes(10));

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllAsync");
                throw new Exception("Failed to load hotels.");
            }
        }

        // ----------------------------------------------
        // Get By Id
        // ----------------------------------------------
        public async Task<HotelDTO?> GetByIdAsync(Guid id)
        {
            try
            {
                string key = CACHE_ITEM + id;

                if (_cache.TryGetValue(key, out HotelDTO? cached))
                    return cached;

                var entity = await _uow.Hotels
                    .Query()
                    .Include(h => h.City)
                        .ThenInclude(c => c.Country)
                    .Include(h => h.Rooms)
                    .FirstOrDefaultAsync(h => h.Id == id);

                if (entity == null)
                    return null;

                var dto = _mapper.Map<HotelDTO>(entity);
                _cache.Set(key, dto, TimeSpan.FromMinutes(10));

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching hotel by ID: {id}");
                throw new Exception("Failed to load hotel details.");
            }
        }

        // ----------------------------------------------
        // CREATE
        // ----------------------------------------------
        public async Task<HotelDTO> CreateAsync(CreateHotelDTO dto)
        {
            try
            {
                var entity = _mapper.Map<Hotel.Entities.Entities.Hotel>(dto);

                await _uow.Hotels.AddAsync(entity);
                await _uow.SaveChangesAsync();

                var fullEntity = await _uow.Hotels
                    .Query()
                    .Include(h => h.City)
                    .ThenInclude(c => c.Country)
                    .FirstOrDefaultAsync(h => h.Id == entity.Id);

                InvalidateCache();

                return _mapper.Map<HotelDTO>(fullEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating hotel.");
                throw new Exception("Failed to create hotel.");
            }
        }

        // ----------------------------------------------
        // UPDATE
        // ----------------------------------------------
        public async Task<HotelDTO> UpdateAsync(UpdateHotelDTO dto)
        {
            try
            {
                var entity = await _uow.Hotels.GetByIdAsync(dto.Id);
                if (entity == null)
                    throw new Exception("Hotel not found.");

                _mapper.Map(dto, entity);

                _uow.Hotels.Update(entity);
                await _uow.SaveChangesAsync();

                var fullEntity = await _uow.Hotels
                    .Query()
                    .Include(h => h.City)
                    .ThenInclude(c => c.Country)
                    .FirstOrDefaultAsync(h => h.Id == entity.Id);

                InvalidateCache();

                return _mapper.Map<HotelDTO>(fullEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating hotel ID {dto.Id}");
                throw new Exception("Failed to update hotel.");
            }
        }

        // ----------------------------------------------
        // DELETE
        // ----------------------------------------------
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
              await _uow.Hotels.DeleteAsync(id);
               await _uow.SaveChangesAsync();
                return true;
                InvalidateCache();

           
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting hotel ID {id}");
                throw new Exception("Failed to delete hotel.");
            }
        }

        // ----------------------------------------------
        // ADD RANGE
        // ----------------------------------------------
        public async Task AddRangeAsync(IEnumerable<CreateHotelDTO> dtos)
        {
            try
            {
                var list = _mapper.Map<List<Hotel.Entities.Entities.Hotel>>(dtos);

                await _uow.Hotels.AddRangeAsync(list);
                await _uow.SaveChangesAsync();

                InvalidateCache();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding multiple hotels.");
                throw new Exception("Failed to add hotels.");
            }
        }

        // ----------------------------------------------
        // SOFT DELETE RANGE
        // ----------------------------------------------
        public async Task<bool> SoftDeleteRangeAsync(IEnumerable<Guid> ids)
        {
            try
            {
                await _uow.Hotels.SoftDeleteRangeAsync(ids);
                await _uow.SaveChangesAsync();

                InvalidateCache();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in soft-delete operation.");
                throw new Exception("Failed to delete hotels.");
            }
        }

        // ----------------------------------------------
        // SEARCH + PAGINATION
        // ----------------------------------------------
        public async Task<PagedResult<HotelDTO>> SearchAsync(
            string? name, Guid? countryId, Guid? cityId, int page, int pageSize)
        {
            try
            {
                var query = _uow.Hotels
                    .Query()
                    .Include(h => h.City)
                    .ThenInclude(c => c.Country)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(h => h.Name.Contains(name));

                if (countryId.HasValue)
                    query = query.Where(h => h.City.CountryId == countryId.Value);

                if (cityId.HasValue)
                    query = query.Where(h => h.CityId == cityId.Value);

                var total = await query.CountAsync();

                var items = await query
                    .OrderBy(h => h.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedResult<HotelDTO>
                {
                    Items = _mapper.Map<List<HotelDTO>>(items),
                    TotalCount = total,
                    Page = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching hotels.");
                throw new Exception("Failed to search hotels.");
            }
        }

        // ----------------------------------------------
        // INVALIDATE CACHE
        // ----------------------------------------------
        private void InvalidateCache()
        {
            _cache.Remove(CACHE_LIST);
        }
    }
}
