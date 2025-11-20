using Hotel.Application.DTOs.BookingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.RoomDto
{
    public class RoomDetailsDTO : RoomDTO
    {
        public List<BookingDTO> Bookings { get; set; }
    }
}
