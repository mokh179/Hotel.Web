using Hotel.Entities.Entities;
using Hotel.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Persistence
{
    public class BookingDBContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public BookingDBContext(DbContextOptions<BookingDBContext> options)
        : base(options) { }

        public DbSet<Hotel.Entities.Entities.Hotel> Hotels => Set<Hotel.Entities.Entities.Hotel>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<RoomType> RoomTypes => Set<RoomType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Soft Delete Filters
            modelBuilder.Entity<Hotel.Entities.Entities.Hotel>().HasQueryFilter(h => !h.IsDeleted);
            modelBuilder.Entity<Room>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<Booking>().HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<Country>().HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<City>().HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<RoomType>().HasQueryFilter(b => !b.IsDeleted);

            modelBuilder.Entity<Hotel.Entities.Entities.Hotel>()
                .HasOne(h => h.City)
                .WithMany(c => c.Hotels)
                .HasForeignKey(h => h.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
          .HasMany(u => u.Bookings)
          .WithOne()  
          .HasForeignKey("UserId") 
          .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
