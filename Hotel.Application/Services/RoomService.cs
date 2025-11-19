using AutoMapper;
using Hotel.Application.DTOs.RoomDto;
using Hotel.Application.Interfaces;
using Hotel.Application.Interfaces.Services;
using Hotel.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Services
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
            var entity = await _unitOfWork.Rooms.GetByIdAsync(id);
            return _mapper.Map<RoomDTO>(entity);
        }

        public async Task<List<RoomDTO>> GetAllAsync()
        {
            var entities = await _unitOfWork.Rooms.GetAllAsync();
            return _mapper.Map<List<RoomDTO>>(entities);
        }

        public async Task<RoomDTO> CreateAsync(CreateRoomDTO dto)
        {
            var entity = _mapper.Map<Room>(dto);
            await _unitOfWork.Rooms.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RoomDTO>(entity);
        }

        public async Task<RoomDTO> UpdateAsync(UpdateRoomDTO dto)
        {
            var entity = await _unitOfWork.Rooms.GetByIdAsync(dto.Id);
            if (entity == null) throw new Exception("Room not found");

            _mapper.Map(dto, entity);
            _unitOfWork.Rooms.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<RoomDTO>(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Rooms.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<RoomDTO>> GetByHotelIdAsync(Guid hotelId)
        {
            var entities = await _unitOfWork.Rooms.FindAsync(r => r.HotelId == hotelId);
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
