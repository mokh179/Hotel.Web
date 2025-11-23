using Hotel.Application.DTOs.RoomDto.RoomTypeDTO;
using Hotel.Application.Interfaces.Services.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.ManageRooms.RoomTypes
{
    [Authorize(Roles = "Admin")]
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly IRoomTypeService _roomTypeService;

        public IndexModel(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        public List<RoomTypeDTO> RoomTypes { get; set; } = new();

        // ------------------------ GET ------------------------
        public async Task OnGetAsync()
        {
            RoomTypes = await _roomTypeService.GetAllAsync();
        }

        // ------------------------ GET BY ID -------------------
        public async Task<JsonResult> OnGetRoomTypeAsync(Guid id)
        {
            var t = await _roomTypeService.GetByIdAsync(id);
            return new JsonResult(t);
        }

        // ------------------------ CREATE ----------------------
        public async Task<JsonResult> OnPostCreateAjaxAsync([FromForm] CreateRoomTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var created = await _roomTypeService.CreateAsync(dto);

                return new JsonResult(new
                {
                    success = true,
                    type = created
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = new[] { ex.Message }
                });
            }
        }

        // ------------------------ EDIT ------------------------
        public async Task<JsonResult> OnPostEditAjaxAsync([FromForm] UpdateRoomTypeDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var updated = await _roomTypeService.UpdateAsync(dto);

                if (updated == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        errors = new[] { "Room type not found." }
                    });
                }

                return new JsonResult(new
                {
                    success = true,
                    type = updated
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = new[] { ex.Message }
                });
            }
        }

        // ------------------------ DELETE ------------------------
        public async Task<JsonResult> OnPostDeleteAjaxAsync([FromQuery] Guid Id)
        {
            try
            {
                var ok = await _roomTypeService.DeleteAsync(Id);

                return new JsonResult(new
                {
                    success = ok
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = new[] { ex.Message }
                });
            }
        }
    }
}
