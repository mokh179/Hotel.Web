using Hotel.Application.DTOs.DateRange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.BookingDto
{
    public class BookingDTO
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid RoomId { get; set; }
        public string RoomName { get; set; }

        public Guid HotelId { get; set; }
        public string HotelName { get; set; }

        public DateRangeDTO DateRange { get; set; }

        public int TotalNights => (DateRange.To - DateRange.From).Days;

        public decimal PricePerNight { get; set; }
        public decimal TotalPrice => TotalNights * PricePerNight;


      

    }
}
