using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.EventHandlers
{
    public class BookingCreatedHandler : INotificationHandler<BookingCreatedNotification>
    {
        private readonly ILogger<BookingCreatedHandler> _logger;

        public BookingCreatedHandler(ILogger<BookingCreatedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(BookingCreatedNotification notification, CancellationToken cancellationToken)
        {
            var booking = notification.DomainEvent.Booking;

            _logger.LogInformation(
                $"[Event] Booking Created: Room {booking.RoomId}, User {booking.UserId}, " +
                $"from {booking.CheckIn} to {booking.CheckOut}");

            // TODO: Send email, update dashboard, global logs, etc.

            return Task.CompletedTask;
        }
    }
}
