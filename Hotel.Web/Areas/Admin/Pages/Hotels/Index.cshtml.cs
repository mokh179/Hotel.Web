using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Locations.City;
using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.DTOs.Pagination;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.Hotels
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHotelService _hotelService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;

        public IndexModel(IHotelService hotelService, ICountryService countryService, ICityService cityService)
        {
            _hotelService = hotelService;
            _countryService = countryService;
            _cityService = cityService;
        }

        public PagedResult<HotelDTO> Paged { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid? CountryId { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid? CityId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public List<CountryDTO> Countries { get; set; } = new();
        public List<CityDTO> CitiesForSelectedCountry { get; set; } = new();

        public async Task OnGetAsync()
        {
            const int pageSize = 9;

            Countries = await _countryService.GetAllAsync();

            if (CountryId.HasValue)
                CitiesForSelectedCountry = await _cityService.GetByCountryAsync(CountryId.Value);

            Paged = await _hotelService.SearchAsync(Search, CountryId, CityId, PageNumber <= 0 ? 1 : PageNumber, pageSize);
        }

        // Return cities for country (AJAX)
        public async Task<JsonResult> OnGetCitiesAsync(Guid countryId)
        {
            var cities = await _cityService.GetByCountryAsync(countryId);
            return new JsonResult(cities.Select(c => new { id = c.Id, name = c.Name }));
        }

        // ----------------------------
        // Load Partial: CREATE
        // ----------------------------
        public async Task<IActionResult> OnGetCreatePartial()
        {
            var model = new CreateHotelDTO
            {
                Countries = await _countryService.GetAllAsync()
            };

            return Partial("_CreateHotel", model);
        }



        
            // ----------------------------
            // Load Partial: EDIT
            // ----------------------------
            public async Task<IActionResult> OnGetEditPartial(Guid id)
            {
                var dto = await _hotelService.GetByIdAsync(id);
                if (dto == null) return NotFound();

                var edit = new UpdateHotelDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Location = dto.Location,
                    CountryId = dto.CountryId,
                    CityId = dto.CityId,
                    Rating = dto.Rating,
                    //Countries = await _locationService.GetAllCountriesAsync()
                };

                return Partial("_EditHotel", edit);
            }


            // ----------------------------
            // Load Partial: DETAILS
            // ----------------------------
            public async Task<IActionResult> OnGetDetailsPartial(Guid id)
            {
                var dto = await _hotelService.GetByIdAsync(id);
                if (dto == null) return NotFound();
                return Partial("_HotelDetails", dto);
            }



            // Return single hotel for edit modal
            public async Task<IActionResult> OnGetHotelAsync(Guid id)
            {
                var hotel = await _hotelService.GetByIdAsync(id);
                return new JsonResult(hotel);
            }

            // AJAX search -> returns PagedResult as JSON (used by client)
            public async Task<JsonResult> OnGetSearchAjaxAsync(string? search, Guid? countryId, Guid? cityId, int page = 1)
            {
                const int pageSize = 9;
                var paged = await _hotelService.SearchAsync(search, countryId, cityId, page <= 0 ? 1 : page, pageSize);

                return new JsonResult(new
                {
                    items = paged.Items,
                    total = paged.TotalCount,
                    page = paged.Page,
                    pageSize = paged.PageSize,
                    totalPages = paged.TotalPages
                });
            }

            // AJAX create
            public async Task<IActionResult> OnPostCreateAjaxAsync([FromForm] CreateHotelDTO dto)
            {
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage) });

                var hotel = await _hotelService.CreateAsync(dto);

                return new JsonResult(new { success = true, hotel });
            }

            // AJAX edit
            public async Task<IActionResult> OnPostEditAjaxAsync([FromForm] UpdateHotelDTO dto)
            {
                if (!ModelState.IsValid)
                    return new JsonResult(new { success = false, errors = ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage) });

                var hotel = await _hotelService.UpdateAsync(dto);

                return new JsonResult(new { success = true, hotel });
            }
            // AJAX delete
            public async Task<JsonResult> OnPostDeleteAjaxAsync(Guid id)
            {
                var ok = await _hotelService.DeleteAsync(id);
                return new JsonResult(new { success = ok });
            }

        } 
    
}
