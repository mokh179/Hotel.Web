using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.RoomDto
{
    public class RoomDTO
    {
        public Guid Id { get; set; }

        public string RoomNumber { get; set; }
        public Guid RoomTypeId { get; set; }  
        public string RoomTypeName { get; set; }  

        public decimal Price { get; set; }

        public Guid HotelId { get; set; }
        public string HotelName { get; set; }

    }

}
