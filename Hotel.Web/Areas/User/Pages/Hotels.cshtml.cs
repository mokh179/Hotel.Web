using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.User.Pages
{
    [Authorize]
    public class HotelsModel : PageModel
    {
        private readonly IHotelService _hotelService;

        public List<HotelDTO> Hotels { get; set; }

        public HotelsModel(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task OnGet()
        {
            Hotels = await _hotelService.GetAllAsync();
        } 
    }
}
