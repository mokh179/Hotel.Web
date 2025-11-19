using Hotel.Application.Interfaces;
using Hotel.Entities.Entities;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelDbContext _context;

        public IGenericRepository<Hotel.Entities.Entities.Hotel> Hotels { get; }
        public IGenericRepository<Room> Rooms { get; }
        public IGenericRepository<Booking> Bookings { get; }

        public IGenericRepository<Country> Countries { get; }

        public IGenericRepository<City> Cities { get; }

        public UnitOfWork(HotelDbContext context)
        {
            _context = context;
            Hotels = new GenericRepository<Hotel.Entities.Entities.Hotel>(_context);
            Rooms = new GenericRepository<Room>(_context);
            Bookings = new GenericRepository<Booking>(_context);
            Cities = new GenericRepository<City>(_context);
            Countries = new GenericRepository<Country>(_context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
     
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
