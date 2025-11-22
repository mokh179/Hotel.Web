using AutoMapper;
using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Pagination;
using Hotel.Application.DTOs.RoomDto;
using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;
using Hotel.Application.Interfaces;
using Hotel.Application.Interfaces.Services;
using Hotel.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Hotel.Infrastructure.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        // -----------------------------------------------------
        // GET BY ID
        // -----------------------------------------------------
        public async Task<RoomDTO> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Rooms
                .Query()
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);

            return _mapper.Map<RoomDTO>(entity);
        }

        // -----------------------------------------------------
        // GET ALL
        // -----------------------------------------------------
        public async Task<List<RoomDTO>> GetAllAsync()
        {
            var entities = await _unitOfWork.Rooms
                .Query()
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .ToListAsync();

            return _mapper.Map<List<RoomDTO>>(entities);
        }

        // -----------------------------------------------------
        // CREATE
        // -----------------------------------------------------
        public async Task<RoomDTO> CreateAsync(CreateRoomDTO dto)
        {
            // Validate unique number inside hotel
            var exists = await _unitOfWork.Rooms
                .Query()
                .AnyAsync(r => r.HotelId == dto.HotelId && r.RoomNumber == dto.RoomNumber);

            if (exists)
                throw new Exception("Room number already exists inside this hotel.");

            var entity = _mapper.Map<Room>(dto);

            await _unitOfWork.Rooms.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            // Reload with relations
            entity = await _unitOfWork.Rooms
                .Query()
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .FirstAsync(r => r.Id == entity.Id);

            return _mapper.Map<RoomDTO>(entity);
        }

        // -----------------------------------------------------
        // UPDATE
        // -----------------------------------------------------
        public async Task<RoomDTO> UpdateAsync(UpdateRoomDTO dto)
        {
            var entity = await _unitOfWork.Rooms.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new Exception("Room not found");

            // Unique validation
            var exists = await _unitOfWork.Rooms
                .Query()
                .AnyAsync(r => r.HotelId == dto.HotelId && r.RoomNumber == dto.RoomNumber && r.Id != dto.Id);

            if (exists)
                throw new Exception("Another room with this number already exists in the hotel.");

            _mapper.Map(dto, entity);

            _unitOfWork.Rooms.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            entity = await _unitOfWork.Rooms
                .Query()
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .FirstAsync(r => r.Id == entity.Id);

            return _mapper.Map<RoomDTO>(entity);
        }

        // -----------------------------------------------------
        // DELETE
        // -----------------------------------------------------
        public async Task<bool> DeleteAsync(Guid id)
        {
            await _unitOfWork.Rooms.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        // -----------------------------------------------------
        // GET ROOMS BY HOTEL
        // -----------------------------------------------------
        public async Task<List<RoomDTO>> GetByHotelIdAsync(Guid hotelId)
        {
            var entities = await _unitOfWork.Rooms
                .Query()
                .Where(r => r.HotelId == hotelId)
                .Include(r => r.RoomType)
                .ToListAsync();

            return _mapper.Map<List<RoomDTO>>(entities);
        }

        // -----------------------------------------------------
        // SEARCH + PAGINATION
        // -----------------------------------------------------
        public async Task<PagedResult<RoomDTO>> SearchRoomsAsync(
            string? number,
            Guid? hotelId,
            Guid? roomTypeId,
            decimal? minPrice,
            decimal? maxPrice,
            bool? available,
            bool? booked,
            int page,
            int pageSize)
        {
            var query = _unitOfWork.Rooms
                .Query()
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(number))
                query = query.Where(r => r.RoomNumber.Contains(number));

            if (hotelId.HasValue)
                query = query.Where(r => r.HotelId == hotelId.Value);

            if (roomTypeId.HasValue)
                query = query.Where(r => r.RoomTypeId == roomTypeId.Value);

            if (minPrice.HasValue)
                query = query.Where(r => r.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(r => r.Price <= maxPrice.Value);

            //// Availability logic:
            //if (available == true)
            //    query = query.Where(r => r.IsAvailable);

            //if (booked == true)
            //    query = query.Where(r => r.IsBooked);

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(r => r.RoomNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<RoomDTO>
            {
                Items = _mapper.Map<List<RoomDTO>>(items),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        // -----------------------------------------------------
        // CACHED ROOMTYPES
        // -----------------------------------------------------
        public async Task<List<RoomTypeDTO>> GetRoomTypesCachedAsync()
        {
            if (_cache.TryGetValue("roomTypes", out List<RoomTypeDTO> cached))
                return cached;

            var types = await _unitOfWork.RoomTypes.Query().ToListAsync();
            var mapped = _mapper.Map<List<RoomTypeDTO>>(types);

            _cache.Set("roomTypes", mapped, TimeSpan.FromMinutes(20));

            return mapped;
        }

        // -----------------------------------------------------
        // CACHED HOTELS
        // -----------------------------------------------------
        public async Task<List<HotelDTO>> GetHotelsCachedAsync()
        {
            if (_cache.TryGetValue("hotelsMin", out List<HotelDTO> cached))
                return cached;

            var hotels = await _unitOfWork.Hotels.Query().ToListAsync();
            var mapped = _mapper.Map<List<HotelDTO>>(hotels);

            _cache.Set("hotelsMin", mapped, TimeSpan.FromMinutes(20));

            return mapped;
        }

        public Task AddRangeAsync(IEnumerable<CreateRoomDTO> dtos)
        {
            throw new NotImplementedException();
        }

        public Task SoftDeleteRangeAsync(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }
    }
}
