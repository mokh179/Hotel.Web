using Hotel.Infrastructure.DependencyInjection;
using Hotel.Infrastructure.Seeders;
using Hotel.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// Razor Pages + Custom Routes
builder.Services.AddRazorPages(options =>
{
    // Account
    options.Conventions.AddAreaPageRoute("Account", "/Login", "login");
    options.Conventions.AddAreaPageRoute("Account", "/Register", "register");

    // user
    options.Conventions.AddAreaPageRoute("User", "/Index", "user");
    options.Conventions.AddAreaPageRoute("User", "/MyBookings", "user/bookings");
    options.Conventions.AddAreaPageRoute("User", "/Profile", "user/profile");
    options.Conventions.AddAreaPageRoute("User", "/Hotels", "user/hotels");
    options.Conventions.AddAreaPageRoute("User", "/HotelDetails", "user/hotel/{id}");

});

// Infrastructure DI
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Roles Seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<BookingDBContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    await RolesSeeder.SeedRolesAsync(context, roleManager);
}

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Razor Pages routing
app.MapRazorPages();

// THE CRITICAL LINE — Area Routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{page?}/{id?}");

app.Run();
