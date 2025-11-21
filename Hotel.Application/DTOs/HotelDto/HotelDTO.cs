using Hotel.Application.DTOs.RoomDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.HotelDto
{
    public class HotelDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }

        public Guid CityId { get; set; }
        public string CityName { get; set; }

        public Guid CountryId { get; set; }   
        public string CountryName { get; set; }

        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double Rating { get; set; }

        public List<RoomDTO> Rooms { get; set; }
        public int RoomCount { get; set; }
    }
}
