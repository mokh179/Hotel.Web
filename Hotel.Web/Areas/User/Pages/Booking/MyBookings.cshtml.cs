using Hotel.Application.DTOs.BookingDto;
using Hotel.Application.DTOs.Pagination;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Hotel.Web.Areas.User.Pages.Booking
{
    [Authorize]
    public class MyBookingsModel : PageModel
    {
        private readonly IBookingService _bookingService;

        //public List<BookingDTO> Bookings { get; set; }
        public PagedResult<BookingDTO> Result { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Hotel { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Room { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? From { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? To { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool Available { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;
        public MyBookingsModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task OnGet()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            Result = await _bookingService.SearchBookingsAsync(
                userId,
                Hotel,
                Room,
                From,
                To,
                Available,
                Page,
                5
            );
        }

        public async Task OnPostCancelAsync(Guid id)
        {
            await _bookingService.DeleteAsync(id);
            Response.Redirect("/User/MyBookings");
        }
    }
}
