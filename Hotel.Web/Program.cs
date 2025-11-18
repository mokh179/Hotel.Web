var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
//builder.Services.AddAutoMapper(typeof(MappingProfile));

//builder.Services.AddScoped<IHotelService, HotelService>();
//builder.Services.AddScoped<IRoomService, RoomService>();
//builder.Services.AddScoped<IBookingService, BookingService>();
//builder.Services.AddScoped<IDomainEventHandler<BookingCreatedEvent>, BookingCreatedEventHandler>();
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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
