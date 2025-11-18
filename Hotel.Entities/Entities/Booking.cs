using Hotel.Entities.DomainEvent.Events;
using Hotel.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Entities.Entities
{
    public class Booking : BaseEntity
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        public Room Room { get; set; }

        [Required]
        public string UserId { get; set; }  // Identity User

        [Required]
        public DateTime CheckIn { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }

        public DateRange DateRange => new DateRange(CheckIn, CheckOut);
        public Booking(int roomId, string userId, DateTime checkIn, DateTime checkOut)
        {
            RoomId = roomId;
            UserId = userId;
            CheckIn = checkIn;
            CheckOut = checkOut;

            AddDomainEvent(new BookingCreatedEvent(this));
        }
    }
}
