using Hotel.Application.DTOs.BookingDto;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.User.Pages.Booking
{
    [Authorize]
    public class MyBookingsModel : PageModel
    {
        private readonly IBookingService _bookingService;

        public List<BookingDTO> Bookings { get; set; }

        public MyBookingsModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task OnGet()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdString, out var userId))
            {
                Bookings = await _bookingService.GetByUserIdAsync(userId);
            }
        }

        public async Task OnPostCancelAsync(Guid id)
        {
            await _bookingService.DeleteAsync(id);
            Response.Redirect("/User/MyBookings");
        }
    }
}
