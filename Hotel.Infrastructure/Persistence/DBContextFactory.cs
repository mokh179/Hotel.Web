using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Persistence
{
    public class HotelDbContextFactory : IDesignTimeDbContextFactory<BookingDBContext>
    {
        public BookingDBContext CreateDbContext(string[] args)
        {
            // Build configuration to read connection string from appsettings.json
            
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Hotel.Web")) // Path depends on where you run the command
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BookingDBContext>();
            var connectionString = configuration.GetConnectionString("DBConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new BookingDBContext(optionsBuilder.Options);
        }
    }
}
