using Hotel.Application.DTOs.Pagination;
using Hotel.Application.DTOs.RoomDto;
using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;
using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Hotel.Application.Interfaces.Services.Profile;

namespace Hotel.Web.Areas.Admin.Pages.ManageRooms.Rooms
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelservice;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IAntiforgery _antiforgery;

        public IndexModel(IRoomService roomService, IHotelService hotelService, IRoomTypeService roomTypeService, IAntiforgery antiforgery)
        {
            _roomService = roomService;
            _hotelservice = hotelService;
            _roomTypeService = roomTypeService;
            _antiforgery = antiforgery;
        }

        public List<HotelDTO> Hotels { get; set; } = new();
        public List<RoomTypeDTO> RoomTypes { get; set; } = new();

        // initial render
        public async Task OnGetAsync()
        {
            // prefer cached lists for dropdowns if available
            try
            {
                // try cache method name first, fallback to GetAllAsync
                var hotelMethod = _hotelservice.GetType().GetMethod("GetAllCachedAsync");
                if (hotelMethod != null)
                    Hotels = await (Task<List<HotelDTO>>)hotelMethod.Invoke(_hotelservice, null);
                else
                    Hotels = await _hotelservice.GetAllAsync();
            }
            catch
            {
                Hotels = await _hotelservice.GetAllAsync();
            }

            try
            {
                var rtMethod = _roomTypeService.GetType().GetMethod("GetAllCachedAsync");
                if (rtMethod != null)
                    RoomTypes = await (Task<List<RoomTypeDTO>>)rtMethod.Invoke(_roomTypeService, null);
                else
                    RoomTypes = await _roomTypeService.GetAllAsync();
            }
            catch
            {
                RoomTypes = await _roomTypeService.GetAllAsync();
            }
        }

        // GET single room
        public async Task<JsonResult> OnGetRoomAsync(Guid id)
        {
            var r = await _roomService.GetByIdAsync(id);
            return new JsonResult(r);
        }

        // AJAX search
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

            var paged = await _roomService.SearchRoomsAsync(number,
                hotelId, roomTypeId,
                priceMin, priceMax,
                isAvailable, null,
                page, pageSize);

            var totalPages = (int)Math.Ceiling((double)paged.TotalCount / pageSize);
            return new JsonResult(new
            {
                items = paged.Items,
                totalCount = paged.TotalCount,
                page = paged.Page,
                pageSize = paged.PageSize,
                totalPages
            });
        }

        // CREATE
        public async Task<JsonResult> OnPostCreateAjaxAsync([FromForm] CreateRoomDTO input)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            try
            {
                var created = await _roomService.CreateAsync(input);
                return new JsonResult(new { success = true, room = created });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }

        // EDIT
        public async Task<JsonResult> OnPostEditAjaxAsync([FromForm] UpdateRoomDTO input)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            try
            {
                var updated = await _roomService.UpdateAsync(input);
                return new JsonResult(new { success = true, room = updated });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }

        // DELETE
        public async Task<JsonResult> OnPostDeleteAjaxAsync([FromForm] Guid Id)
        {
            try
            {
                var ok = await _roomService.DeleteAsync(Id);
                return new JsonResult(new { success = ok });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }
    }
}
