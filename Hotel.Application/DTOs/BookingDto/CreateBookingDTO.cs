using Hotel.Application.DTOs.DateRange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.BookingDto
{
    public class CreateBookingDTO
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public DateRangeDTO DateRange { get; set; }
    }
}
