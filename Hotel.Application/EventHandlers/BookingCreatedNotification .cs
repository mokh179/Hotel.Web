using Hotel.Entities.DomainEvent.Events;
using Hotel.Entities.DomainEvent.Intrefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Hotel.Application.EventHandlers
{
    public class BookingCreatedNotification : INotification
    {
        public BookingCreatedEvent DomainEvent { get; }

        public BookingCreatedNotification(BookingCreatedEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }
    }
}
