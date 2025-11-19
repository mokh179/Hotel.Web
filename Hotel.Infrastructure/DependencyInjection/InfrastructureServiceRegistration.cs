using Hotel.Application.Interfaces.Services;
using Hotel.Application.Interfaces;
using Hotel.Application.Mapping;
using Hotel.Application.Services;
using Hotel.Infrastructure.Identity;
using Hotel.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //looging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

           // builder.Host.UseSerilog();

            //DB Context
            services.AddDbContext<HotelDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Identity
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<HotelDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            //Mapper
            services.AddAutoMapper(typeof(MappingProfile));

            //DI
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingService, BookingService>();
        }
    }
}
