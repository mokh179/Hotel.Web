using AutoMapper;
using Hotel.Application.DTOs.RoomDto;
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
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<RoomDTO> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Rooms
                .Query()
                .Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.Id == id);

            return _mapper.Map<RoomDTO>(entity);
        }


        public async Task<List<RoomDTO>> GetAllAsync()
        {
            var entities = await _unitOfWork.Rooms
                .Query()
                .Include(r => r.Hotel)
                .ToListAsync();

            return _mapper.Map<List<RoomDTO>>(entities);
        }

        public async Task<RoomDTO> CreateAsync(CreateRoomDTO dto)
        {
            var entity = _mapper.Map<Room>(dto);

            await _unitOfWork.Rooms.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            entity = await _unitOfWork.Rooms
                .Query()
                .Include(r => r.Hotel)
                .FirstAsync(r => r.Id == entity.Id);

            return _mapper.Map<RoomDTO>(entity);
        }

    
        public async Task<RoomDTO> UpdateAsync(UpdateRoomDTO dto)
        {
            var entity = await _unitOfWork.Rooms.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new Exception("Room not found");

            _mapper.Map(dto, entity);

            _unitOfWork.Rooms.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            // Reload with Hotel
            entity = await _unitOfWork.Rooms
                .Query()
                .Include(r => r.Hotel)
                .FirstAsync(r => r.Id == entity.Id);

            return _mapper.Map<RoomDTO>(entity);
        }


        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Rooms.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

   
        public async Task<List<RoomDTO>> GetByHotelIdAsync(Guid hotelId)
        {
            var entities = await _unitOfWork.Rooms
                .Query()
                .Where(r => r.HotelId == hotelId)
                .ToListAsync();

            return _mapper.Map<List<RoomDTO>>(entities);
        }


        public async Task AddRangeAsync(IEnumerable<CreateRoomDTO> dtos)
        {
            var entities = _mapper.Map<IEnumerable<Room>>(dtos);

            await _unitOfWork.Rooms.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
        }

   
        public async Task SoftDeleteRangeAsync(IEnumerable<Guid> ids)
        {
            await _unitOfWork.Rooms.SoftDeleteRangeAsync(ids);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
