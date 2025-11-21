using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hotel.Application.DTOs.BookingDto;
using Hotel.Application.DTOs.DateRange;
using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Locations.City;
using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.DTOs.RoomDto;
using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;
using Hotel.Entities.Entities;
using Hotel.Entities.ValueObjects;

namespace Hotel.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {

            #region Hotel
            CreateMap<Hotel.Entities.Entities.Hotel, HotelDTO>()
                .ForMember(dest => dest.CityName,
                    opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CountryName,
                    opt => opt.MapFrom(src => src.City.Country.Name))
                .ForMember(dest => dest.CountryId,
                    opt => opt.MapFrom(src => src.City.CountryId))       
                .ForMember(dest => dest.Rooms,
                    opt => opt.MapFrom(src => src.Rooms))
                .ForMember(dest => dest.RoomCount,
                    opt => opt.MapFrom(src => src.Rooms.Count)).ReverseMap();

            CreateMap<CreateHotelDTO, Hotel.Entities.Entities.Hotel>().ReverseMap();
            CreateMap<UpdateHotelDTO, Hotel.Entities.Entities.Hotel>().ReverseMap();
            #endregion



            #region Room
            CreateMap<Room, RoomDTO>()
                    .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
                    .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name));

            CreateMap<CreateRoomDTO, Room>();
            CreateMap<UpdateRoomDTO, Room>();
            #endregion




            #region Booking
            CreateMap<Booking, BookingDTO>()
                                         .ForMember(dest => dest.RoomName,
                                                    opt => opt.MapFrom(src => src.Room.RoomNumber))
                                         .ForMember(dest => dest.HotelId,
                                                    opt => opt.MapFrom(src => src.Room.Hotel.Id))
                                         .ForMember(dest => dest.HotelName,
                                                    opt => opt.MapFrom(src => src.Room.Hotel.Name))
                                         .ForMember(dest => dest.PricePerNight,
                                                    opt => opt.MapFrom(src => src.Room.Price))
                                         .ForMember(dest => dest.DateRange,
                                                    opt => opt.MapFrom(src => new DateRangeDTO
                                                    {
                                                        From = src.CheckIn,
                                                        To = src.CheckOut
                                                    }));

            CreateMap<CreateBookingDTO, Booking>();
            CreateMap<UpdateBookingDTO, Booking>();
            #endregion

            #region Country
            CreateMap<Country, CountryDTO>();
            CreateMap<CreateCountryDTO, Country>();
            CreateMap<UpdateCountryDTO, Country>();
            #endregion

            #region City
            CreateMap<City, CityDTO>();
            CreateMap<CreateCityDTO, City>();
            CreateMap<UpdateCityDTO, City>();
            #endregion

            CreateMap<RoomType, RoomTypeDTO>();
            CreateMap<CreateRoomTypeDTO, RoomType>();
            CreateMap<UpdateRoomTypeDTO, RoomType>();

            CreateMap<DateRange, DateRangeDTO>().ReverseMap();
        }
                
    }
}
