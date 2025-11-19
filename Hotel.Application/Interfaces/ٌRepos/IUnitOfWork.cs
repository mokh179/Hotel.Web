using Hotel.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Hotel.Entities.Entities.Hotel> Hotels { get; }
        IGenericRepository<Room> Rooms { get; }
        IGenericRepository<Booking> Bookings { get; }
         IGenericRepository<Country> Countries { get; }
         IGenericRepository<City> Cities { get; }
        Task<int> SaveChangesAsync();
    }
}
