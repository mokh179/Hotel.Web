using Hotel.Application.DTOs.BookingDto;
using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.Interfaces.Services;
using Hotel.Application.Interfaces.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Hotel.Web.Areas.User.Pages
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly IHotelService _hotelService;

        public DashboardModel(IBookingService bookingService, IUserService userService, IHotelService hotelService)
        {
            _bookingService = bookingService;
            _userService = userService;
            _hotelService = hotelService;
        }

        public string FirstName { get; set; }
        public int ActiveBookings { get; set; }
        public int PastBookings { get; set; }
        public int TotalBookings { get; set; }
        public List<BookingDTO> RecentBookings { get; set; } = new();
        public List<HotelDTO> SuggestedHotels { get; set; } = new();

        public async Task OnGet()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Profile
            var profile = await _userService.GetProfileAsync(userId);
            FirstName = profile.FirstName;

            // Stats
            ActiveBookings = await _bookingService.CountActiveBookingsAsync(userId);
            PastBookings = await _bookingService.CountPastBookingsAsync(userId);

            var allBookings = await _bookingService.GetByUserIdAsync(userId);

            TotalBookings = allBookings.Count;

            RecentBookings = allBookings
                .OrderByDescending(b => b.DateRange.From)
                .Take(3)
                .ToList();

            // Suggested Hotels
            var allHotels = await _hotelService.GetAllAsync();
            SuggestedHotels = allHotels
                .OrderByDescending(h => h.Rating)
                .Take(3)
                .ToList();
        }
    }
}
