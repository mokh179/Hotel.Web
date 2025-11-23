using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Pagination;
using Hotel.Application.DTOs.RoomDto;
using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;
using Hotel.Application.Interfaces.Services;
using Hotel.Application.Interfaces.Services.Profile;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.ManageRooms.Rooms
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IAntiforgery _anti;

        public IndexModel(IRoomService roomService,
                          IHotelService hotelService,
                          IRoomTypeService roomTypeService,
                          IAntiforgery anti)
        {
            _roomService = roomService;
            _hotelService = hotelService;
            _roomTypeService = roomTypeService;
            _anti = anti;
        }

        public List<HotelDTO> Hotels { get; set; } = new();
        public List<RoomTypeDTO> RoomTypes { get; set; } = new();

        public async Task OnGetAsync()
        {
            Hotels = await _hotelService.GetAllAsync();
            RoomTypes = await _roomTypeService.GetAllAsync();
        }

        // ---------------------------------------------
        // GET: Partial Create
        // ---------------------------------------------
        public async Task<PartialViewResult> OnGetCreatePartial()
        {
            var vm = new _CreateRoomModel();
            Hotels = await _hotelService.GetAllAsync();
            RoomTypes = await _roomTypeService.GetAllAsync();
            vm.Hotels = Hotels;
            vm.RoomTypes = RoomTypes;
            return Partial("_CreateRoom", vm);
        }

        // ---------------------------------------------
        // GET: Partial Edit
        // ---------------------------------------------
        public async Task<PartialViewResult> OnGetEditPartialAsync(Guid id)
        {
            var room = await _roomService.GetByIdAsync(id);
            Hotels = await _hotelService.GetAllAsync();
            RoomTypes = await _roomTypeService.GetAllAsync();
            var vm = new _EditRoomModel
            {
                Room = new UpdateRoomDTO
                {
                    Id = room.Id,
                    RoomNumber = room.RoomNumber,
                    Price = room.Price,
                    HotelId = room.HotelId,
                    RoomTypeId = room.RoomTypeId,
                    //Description = room.Description
                },
                Hotels = Hotels,
                RoomTypes = RoomTypes
            };
            

            return Partial("_EditRoom", vm);
        }

        // ---------------------------------------------
        // GET: Details Partial
        // ---------------------------------------------
        public async Task<PartialViewResult> OnGetDetailsPartialAsync(Guid id)
        {
            var room = await _roomService.GetByIdAsync(id);
            return Partial("_RoomDetails", room);
        }

        // ---------------------------------------------
        // AJAX Search + Pagination
        // ---------------------------------------------
        public async Task<JsonResult> OnGetSearchAjaxAsync(
            string number,
            Guid? hotelId,
            Guid? roomTypeId,
            decimal? priceMin,
            decimal? priceMax,
            string availability,
            int page = 1,
            int pageSize = 9)
        {
            bool? isAvailable = null;

            if (!string.IsNullOrWhiteSpace(availability))
            {
                if (availability == "available") isAvailable = true;
                else if (availability == "booked") isAvailable = false;
            }

            var result = await _roomService.SearchRoomsAsync(
                number,
                hotelId,
                roomTypeId,
                priceMin,
                priceMax,
                isAvailable,
                null,
                page,
                pageSize
            );

            int totalPages = (int)Math.Ceiling((double)result.TotalCount / pageSize);

            return new JsonResult(new
            {
                items = result.Items,
                result.Page,
                result.PageSize,
                totalPages
            });
        }

        // ---------------------------------------------
        // CREATE AJAX
        // ---------------------------------------------
        public async Task<JsonResult> OnPostCreateAjaxAsync([FromForm] CreateRoomDTO dto)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage)
                });

            try
            {
                var room = await _roomService.CreateAsync(dto);
                return new JsonResult(new { success = true, room });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }

        // ---------------------------------------------
        // EDIT AJAX
        // ---------------------------------------------
        public async Task<JsonResult> OnPostEditAjaxAsync([FromForm] UpdateRoomDTO dto)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage)
                });

            try
            {
                var room = await _roomService.UpdateAsync(dto);
                return new JsonResult(new { success = true, room });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }

        // ---------------------------------------------
        // DELETE AJAX
        // ---------------------------------------------
        public async Task<JsonResult> OnPostDeleteAjaxAsync([FromForm] Guid id)
        {
            try
            {
                bool ok = await _roomService.DeleteAsync(id);
                return new JsonResult(new { success = ok });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }
    }
}
