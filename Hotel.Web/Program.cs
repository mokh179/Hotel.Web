using Hotel.Infrastructure.DependencyInjection;
using Hotel.Infrastructure.Seeders;
using Hotel.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddRazorPages();

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<BookingDBContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

    await RolesSeeder.SeedRolesAsync(context, roleManager);
}

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

app.MapRazorPages();

app.Run();
