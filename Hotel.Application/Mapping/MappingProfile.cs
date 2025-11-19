using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hotel.Application.DTOs.BookingDto;
using Hotel.Application.DTOs.DateRange;
using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.RoomDto;
using Hotel.Entities.Entities;
using Hotel.Entities.ValueObjects;

namespace Hotel.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {

            CreateMap<Hotel.Entities.Entities.Hotel, HotelDTO>()
                                                                 .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                                                                 .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.City.Country.Name)); ;
            CreateMap<CreateHotelDTO, Hotel.Entities.Entities.Hotel>();
            CreateMap<UpdateHotelDTO, Hotel.Entities.Entities.Hotel>();

            CreateMap<Room, RoomDTO>().ReverseMap();
            CreateMap<CreateRoomDTO, Room>();
            CreateMap<UpdateRoomDTO, Room>();

            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<CreateBookingDTO, Booking>();
            CreateMap<UpdateBookingDTO, Booking>();


            CreateMap<DateRange, DateRangeDTO>().ReverseMap();
        }
                
    }
}
