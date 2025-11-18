using Hotel.Entities.DomainEvent.Intrefaces;
using Hotel.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Entities.DomainEvent.Events
{
    public class BookingCreatedEvent:IDomainEvent
    {
        public Booking Booking { get; }

        public BookingCreatedEvent(Booking booking)
        {
            Booking = booking;
        }
    }
}
