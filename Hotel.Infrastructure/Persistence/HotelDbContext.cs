using Hotel.Entities.Entities;
using Hotel.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Persistence
{
    public class HotelDbContext : IdentityDbContext<ApplicationUser>
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options) { }

        public DbSet<Hotel.Entities.Entities.Hotel> Hotels => Set<Hotel.Entities.Entities.Hotel>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Soft Delete Filters
            modelBuilder.Entity<Hotel.Entities.Entities.Hotel>().HasQueryFilter(h => !h.IsDeleted);
            modelBuilder.Entity<Room>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<Booking>().HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
