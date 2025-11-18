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
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Create
        public async Task AddRoomAsync(RoomDTO roomDto)
        {
            var room = _mapper.Map<Room>(roomDto);
            await _unitOfWork.Rooms.AddAsync(room);
            await _unitOfWork.SaveChangesAsync();
        }

        // Read All by Hotel
        public async Task<IEnumerable<RoomDTO>> GetRoomsByHotelAsync(int hotelId)
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            return _mapper.Map<IEnumerable<RoomDTO>>(rooms.Where(r => r.HotelId == hotelId));
        }

        // Read by Id
        public async Task<RoomDTO> GetRoomByIdAsync(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            return _mapper.Map<RoomDTO>(room);
        }

        // Update
        public async Task UpdateRoomAsync(int roomId, RoomDTO roomDto)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null) throw new Exception("Room not found");

            _mapper.Map(roomDto, room);
            _unitOfWork.Rooms.Update(room);
            await _unitOfWork.SaveChangesAsync();
        }

        // Delete (Soft Delete)
        public async Task DeleteRoomAsync(int roomId)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            if (room == null) throw new Exception("Room not found");

            _unitOfWork.Rooms.Delete(room);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddRoomsAsync(IEnumerable<RoomDTO> roomDtos)
        {
            var rooms = _mapper.Map<IEnumerable<Room>>(roomDtos);
            await _unitOfWork.Rooms.AddRangeAsync(rooms);
            await _unitOfWork.SaveChangesAsync();
        }

        // Bulk Update
        public async Task UpdateRoomsAsync(IEnumerable<RoomDTO> roomDtos)
        {
            foreach (var dto in roomDtos)
            {
                await _unitOfWork.Rooms.Entities
                    .Where(r => r.Id == dto.Id)
                    .ExecuteUpdateAsync(r => r
                        .SetProperty(p => p.RoomNumber, dto.RoomNumber)
                        .SetProperty(p => p.Price, dto.Price)
                    );
            }
        }

        // Bulk Delete (Soft Delete)
        public async Task DeleteRoomsAsync(IEnumerable<int> roomIds)
        {
            await _unitOfWork.Rooms.Entities
                .Where(r => roomIds.Contains(r.Id))
                .ExecuteUpdateAsync(r => r
                    .SetProperty(p => p.IsDeleted, true)
                    .SetProperty(p => p.DeletedAt, DateTime.UtcNow)
                );
        }
    }

}
