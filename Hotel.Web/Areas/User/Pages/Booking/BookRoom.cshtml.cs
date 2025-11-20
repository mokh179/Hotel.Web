using Hotel.Application.DTOs.BookingDto;
using Hotel.Application.DTOs.DateRange;
using Hotel.Application.DTOs.RoomDto;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.User.Pages.Booking
{
    [Authorize]
    public class BookRoomModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;

        public RoomDTO Room { get; set; }

        [BindProperty]
        public BookingInputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public BookRoomModel(IRoomService roomService, IBookingService bookingService)
        {
            _roomService = roomService;
            _bookingService = bookingService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Room = await _roomService.GetByIdAsync(id);
            if (Room == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            Room = await _roomService.GetByIdAsync(id);
            if (Room == null) return NotFound();

            if (Input.CheckIn >= Input.CheckOut)
            {
                ErrorMessage = "Check-out date must be after check-in date.";
                return Page();
            }

            bool available = await _bookingService.IsRoomAvailable(id, Input.CheckIn, Input.CheckOut);

            if (!available)
            {
                ErrorMessage = "This room is not available in the selected dates.";
                return Page();
            }

            var userId = Guid.Parse(User.FindFirst("sub")?.Value);

            var dto = new CreateBookingDTO
            {
                RoomId = id,
                UserId = userId,
                DateRange=new DateRangeDTO() { From=Input.CheckIn, To=Input.CheckOut },
            };

            await _bookingService.CreateAsync(dto);

            return RedirectToPage("/BookingConfirmation", new { area = "User" });
        }

        public class BookingInputModel
        {
            public DateTime CheckIn { get; set; }
            public DateTime CheckOut { get; set; }
        }
    }
}
