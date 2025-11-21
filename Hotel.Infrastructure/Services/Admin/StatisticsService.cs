using Hotel.Application.DTOs.Admin;
using Hotel.Application.Interfaces.Services;
using Hotel.Application.Interfaces.Services.Admin;
using Hotel.Application.Interfaces.Services.Profile;
using Microsoft.Extensions.Caching.Memory;

namespace Hotel.Infrastructure.Services.Admin
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

        private const string CacheKey = "STATISTICS_CACHE";

        public StatisticsService(
            IHotelService hotelService,
            IRoomService roomService,
            IBookingService bookingService,
            IUserService userService,
            IMemoryCache cache)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _bookingService = bookingService;
            _userService = userService;
            _cache = cache;
        }

        public async Task<StatisticsDTO> GetSnapshotAsync()
        {
            if (_cache.TryGetValue(CacheKey, out StatisticsDTO cached))
                return cached;

            var hotels = await _hotelService.GetAllAsync();
            var rooms = await _roomService.GetAllAsync();
            var bookings = await _bookingService.GetAllAsync();
            var users = await _userService.GetAllAsync();

            var snapshot = new StatisticsDTO
            {
                Hotels = hotels,
                Rooms = rooms,
                Bookings = bookings,
                Users = users
            };

            _cache.Set(CacheKey, snapshot,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                });

            return snapshot;
        }

        public async Task<StatisticsDTO> GetDashboardAsync()
        {
            var snapshot = await GetSnapshotAsync();

            var hotels = snapshot.Hotels;
            var rooms = snapshot.Rooms;
            var bookings = snapshot.Bookings;
            var users = snapshot.Users;

            var today = DateTime.UtcNow.Date;

            snapshot.TotalHotels = hotels.Count;
            snapshot.TotalRooms = rooms.Count;
            snapshot.TotalBookings = bookings.Count;
            snapshot.TotalUsers = users.Count;

            snapshot.TodayBookings = bookings.Count(b => b.DateRange.From.Date == today);
            snapshot.FutureBookings = bookings.Count(b => b.DateRange.From.Date > today);
            snapshot.PastBookings = bookings.Count(b => b.DateRange.To.Date < today);

            snapshot.TopHotelName =
                bookings.GroupBy(b => b.HotelId)
                        .OrderByDescending(g => g.Count())
                        .Select(g => hotels.FirstOrDefault(h => h.Id == g.Key)?.Name)
                        .FirstOrDefault() ?? "No data";

            snapshot.MostBookedRoomType =
                bookings.GroupBy(b => b.RoomId)
                        .OrderByDescending(g => g.Count())
                        .Select(g =>
                        {
                            var room = rooms.FirstOrDefault(r => r.Id == g.Key);
                            return room?.RoomType.ToString();
                        })
                        .FirstOrDefault() ?? "No data";

            snapshot.TotalRevenue = bookings.Sum(b => b.TotalPrice);

            snapshot.AverageStayLength =
                bookings.Any()
                ? bookings.Average(b => (b.DateRange.To - b.DateRange.From).TotalDays)
                : 0;

            snapshot.RecentBookings = bookings
                .OrderByDescending(b => b.DateRange.From)
                .Take(5)
                .ToList();

            snapshot.RecentUsers = users
                .OrderByDescending(x=>x.CreatedAt)
                .Take(5)
                .ToList();

            return snapshot;
        }

        public void ClearCache() => _cache.Remove(CacheKey);
    }
}
