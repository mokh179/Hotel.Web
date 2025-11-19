using Hotel.Entities.Enums;
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
        public Guid HotelId { get; set; }
        public string RoomNumber { get; set; }
        public decimal Price { get; set; }
        public RoomType RoomType { get; set; }
    }
}
