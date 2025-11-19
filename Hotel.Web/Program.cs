using Hotel.Application.Interfaces.Services;
using Hotel.Application.Interfaces;
using Hotel.Application.Mapping;
using Hotel.Application.Services;
using Hotel.Infrastructure.Identity;
using Hotel.Infrastructure.Persistence;
using Hotel.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Hotel.Infrastructure.DependencyInjection;
using Hotel.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    await RolesSeeder.SeedRolesAsync(roleManager);
}
builder.Host.UseSerilog();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
