using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;
using Hotel.Application.DTOs.RoomDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.ManageRooms.Rooms
{
    public class _CreateRoomModel : PageModel
    {
        [BindProperty]
        public CreateRoomDTO Room { get; set; } = new();

        public List<HotelDTO> Hotels { get; set; } = new();
        public List<RoomTypeDTO> RoomTypes { get; set; } = new();

        public void OnGet(List<HotelDTO> hotels, List<RoomTypeDTO> roomTypes)
        {
            Hotels = hotels;
            RoomTypes = roomTypes;
        }
    }
}
