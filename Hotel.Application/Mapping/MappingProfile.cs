using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hotel.Application.DTOs;
using Hotel.Entities.Entities;

namespace Hotel.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Hotel.Entities.Entities.Hotel, HotelDTO>().ReverseMap();
            CreateMap<Room, RoomDTO>().ReverseMap();
        
            // Booking
            // Booking عندها Constructor يحتاج Parameters، لذلك نستخدم ConstructUsing
            CreateMap<BookingDTO, Booking>()
                .ConstructUsing(dto => new Booking(
                    dto.RoomId,
                    dto.UserId,
                    dto.CheckIn,
                    dto.CheckOut
                ));    
        }
                
        }
}
