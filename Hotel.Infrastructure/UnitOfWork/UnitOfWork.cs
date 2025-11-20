using Hotel.Application.EventHandlers;
using Hotel.Application.Interfaces;
using Hotel.Entities.Entities;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Hotel.Entities.DomainEvent.Intrefaces;


namespace Hotel.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookingDBContext _context;
        private readonly IMediator _mediator;

        public IGenericRepository<Hotel.Entities.Entities.Hotel> Hotels { get; }
        public IGenericRepository<Room> Rooms { get; }
        public IGenericRepository<Booking> Bookings { get; }

        public IGenericRepository<Country> Countries { get; }

        public IGenericRepository<City> Cities { get; }

        public UnitOfWork(BookingDBContext context, IMediator mediator)
        {
            _context = context;
            Hotels = new GenericRepository<Hotel.Entities.Entities.Hotel>(_context);
            Rooms = new GenericRepository<Room>(_context);
            Bookings = new GenericRepository<Booking>(_context);
            Cities = new GenericRepository<City>(_context);
            Countries = new GenericRepository<Country>(_context);
            _mediator = mediator;
        }

       // public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
        public async Task<int> SaveChangesAsync() 
        {
            // 1) Gather Domain Events from tracked entities
            var domainEvents = _context.ChangeTracker
                .Entries<BaseEntity>()
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            // 2) Clear events before saving
            foreach (var entity in _context.ChangeTracker.Entries<BaseEntity>())
                entity.Entity.ClearDomainEvents();

            // 3) Commit to DB
            int result = await _context.SaveChangesAsync();

            // 4) Publish events using MediatR
            foreach (var domainEvent in domainEvents)
            {
                var notification = new BookingCreatedNotification((dynamic)domainEvent);
                await _mediator.Publish(notification);
            }

            return result;
        }
        private INotification CreateNotification(IDomainEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(
                typeof(BookingCreatedNotification), domainEvent)!;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
