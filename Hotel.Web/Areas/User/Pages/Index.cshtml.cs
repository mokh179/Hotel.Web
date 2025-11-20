using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.User.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IBookingService _bookingService;

        public int CurrentBookings { get; set; }
        public int PastBookings { get; set; }

        public IndexModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task OnGet()
        {
            var userId = User.FindFirst("sub")?.Value
                         ?? User.FindFirst("id")?.Value
                         ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userId, out var guid))
            {
                CurrentBookings = await _bookingService.CountActiveBookingsAsync(guid);
                PastBookings = await _bookingService.CountPastBookingsAsync(guid);
            }
        }
    }
}
