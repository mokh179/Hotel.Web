using AutoMapper;
using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Pagination;
using Hotel.Application.Interfaces;
using Hotel.Application.Interfaces.Services;
using Hotel.Entities.Entities;
using Hotel.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public class HotelService : IHotelService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;

    private const string CacheKey_AllHotels = "HOTELS_ALL";

    public HotelService(IUnitOfWork unitOfWork, IMapper mapper, IDistributedCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    // -------------------- GET ALL --------------------
    public async Task<List<HotelDTO>> GetAllAsync()
    {
        var hotels = await _unitOfWork.Hotels.GetAllAsync();
        return _mapper.Map<List<HotelDTO>>(hotels);
    }

    // -------------------- GET ALL (CACHED) --------------------
    public async Task<List<HotelDTO>> GetAllCachedAsync()
    {
        var cacheData = await _cache.GetStringAsync(CacheKey_AllHotels);

        if (!string.IsNullOrEmpty(cacheData))
            return JsonSerializer.Deserialize<List<HotelDTO>>(cacheData)!;

        var hotels = await GetAllAsync();

        await _cache.SetStringAsync(
            CacheKey_AllHotels,
            JsonSerializer.Serialize(hotels),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });

        return hotels;
    }

    // -------------------- GET BY ID --------------------
    public async Task<HotelDTO> GetByIdAsync(Guid id)
    {
        var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);
        if (hotel == null)
            throw new Exception("Hotel not found");

        return _mapper.Map<HotelDTO>(hotel);
    }

    // -------------------- CREATE --------------------
    public async Task<HotelDTO> CreateAsync(CreateHotelDTO dto)
    {
        bool exists = await _unitOfWork.Hotels.ExistsAsync(h => h.Name == dto.Name);
        if (exists)
            throw new Exception("Hotel with same name already exists");

        var entity = _mapper.Map<Hotel.Entities.Entities.Hotel>(dto);

        await _unitOfWork.Hotels.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        await _cache.RemoveAsync(CacheKey_AllHotels);

        return _mapper.Map<HotelDTO>(entity);
    }

    // -------------------- UPDATE --------------------
    public async Task<HotelDTO> UpdateAsync(UpdateHotelDTO dto)
    {
        var hotel = await _unitOfWork.Hotels.GetByIdAsync(dto.Id);
        if (hotel == null)
            throw new Exception("Hotel not found");

        bool exists = await _unitOfWork.Hotels.ExistsAsync(h => h.Name == dto.Name && h.Id != dto.Id);
        if (exists)
            throw new Exception("Another hotel with same name exists");

        _mapper.Map(dto, hotel);

        _unitOfWork.Hotels.Update(hotel);
        await _unitOfWork.SaveChangesAsync();

        await _cache.RemoveAsync(CacheKey_AllHotels);

        return _mapper.Map<HotelDTO>(hotel);
    }

    // -------------------- DELETE --------------------
    public async Task<bool> DeleteAsync(Guid id)
    {
        await _unitOfWork.Hotels.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        await _cache.RemoveAsync(CacheKey_AllHotels);

        return true;
    }

    // -------------------- SEARCH WITH PAGINATION --------------------
    public async Task<PagedResult<HotelDTO>> SearchHotelsAsync(string name, Guid? countryId, Guid? cityId, int page, int pageSize) 
    { 
        var query = _unitOfWork.Hotels.Query()
            .Include(h => h.City).
            ThenInclude(c => c.Country).
            AsQueryable();
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(h => h.Name.Contains(name)); 
        if (countryId.HasValue) 
            query = query.Where(h => h.City.CountryId == countryId.Value); 
        if (cityId.HasValue) query = query.Where(h => h.CityId == cityId.Value); 
        var total = await query.CountAsync(); 
        var hotels = await query.OrderBy(h => h.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(); return new PagedResult<HotelDTO> { Items = _mapper.Map<List<HotelDTO>>(hotels), TotalCount = total, Page = page, PageSize = pageSize };
    }
    public async Task SoftDeleteRangeAsync(IEnumerable<Guid> ids) { await _unitOfWork.Hotels.SoftDeleteRangeAsync(ids); await _unitOfWork.SaveChangesAsync(); }

    public async Task AddRangeAsync(IEnumerable<CreateHotelDTO> dtos) { var entities = _mapper.Map<IEnumerable<Hotel.Entities.Entities.Hotel>>(dtos); await _unitOfWork.Hotels.AddRangeAsync(entities); await _unitOfWork.SaveChangesAsync(); }

}
