using Hotel.Application.Interfaces.Services;
using Hotel.Application.Interfaces;
using Hotel.Application.Mapping;
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
using Hotel.Infrastructure.Repositories;
using Hotel.Infrastructure.Services;
using Hotel.Application.Interfaces.Services.Authentication;
using Hotel.Infrastructure.Services.Auth;

namespace Hotel.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<BookingDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DBConnection")));

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<BookingDBContext>()
                .AddDefaultTokenProviders();

            // Authentication + JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
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

            services.AddAuthorization();

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Repositories + UoW + Services
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
