using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.User.Pages.Hotels
{
    [Authorize]
    public class HotelDetailsModel : PageModel
    {
        private readonly IHotelService _hotelService;

        public HotelDTO Hotel { get; set; }

        public HotelDetailsModel(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Hotel = await _hotelService.GetByIdAsync(id);

            if (Hotel == null)
                return NotFound();

            return Page();
        }
    }
}
