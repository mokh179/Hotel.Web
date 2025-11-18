using Hotel.Entities.DomainEvent.Events;
using Hotel.Entities.DomainEvent.Intrefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.EventHandlers
{
    public class BookingCreatedEventHandler : IDomainEventHandler<BookingCreatedEvent>
    {
        public Task Handle(BookingCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Booking created for user {domainEvent.Booking.UserId}, Room {domainEvent.Booking.RoomId}");
            return Task.CompletedTask;
        }
    }
}
