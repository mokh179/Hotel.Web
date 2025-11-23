using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;
using Hotel.Application.DTOs.RoomDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.ManageRooms.Rooms
{
    public class _EditRoomModel : PageModel
    {
        [BindProperty]
        public UpdateRoomDTO Room { get; set; } = new();

        public List<HotelDTO> Hotels { get; set; } = new();
        public List<RoomTypeDTO> RoomTypes { get; set; } = new();

        public void OnGet(UpdateRoomDTO room, List<HotelDTO> hotels, List<RoomTypeDTO> roomTypes)
        {
            Room = room;
            Hotels = hotels;
            RoomTypes = roomTypes;
        }
    }
}
