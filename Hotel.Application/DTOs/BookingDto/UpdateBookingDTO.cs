using Hotel.Application.DTOs.DateRange;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.BookingDto
{
    public class UpdateBookingDTO
    {
        public Guid Id { get; set; }
        [Required]
        public Guid RoomId { get; set; }
        public DateRangeDTO DateRange { get; set; }
    } 
}
