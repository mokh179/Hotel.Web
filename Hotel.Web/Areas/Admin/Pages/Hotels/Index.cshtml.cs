using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Locations.City;
using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.DTOs.Pagination;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.Hotel
{
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

        [BindProperty(SupportsGet = true)] public string? Search { get; set; }
        [BindProperty(SupportsGet = true)] public Guid? CountryId { get; set; }
        [BindProperty(SupportsGet = true)] public Guid? CityId { get; set; }
        [BindProperty(SupportsGet = true)] public int Page { get; set; } = 1;

        public List<CountryDTO> Countries { get; set; } = new();
        public List<CityDTO> CitiesForSelectedCountry { get; set; } = new();

        public async Task OnGetAsync()
        {
            const int pageSize = 8;

            Countries = await _countryService.GetAllAsync();

            if (CountryId.HasValue)
                CitiesForSelectedCountry = await _cityService.GetByCountryAsync(CountryId.Value);

            Paged = await _hotelService.SearchHotelsAsync(
                Search ?? string.Empty,
                CountryId,
                CityId,
                Page <= 0 ? 1 : Page,
                pageSize
            );
        }

        public async Task<JsonResult> OnGetCitiesAsync(Guid countryId)
        {
            var cities = await _cityService.GetByCountryAsync(countryId);
            return new JsonResult(cities);
        }

        public async Task<JsonResult> OnGetHotelAsync(Guid id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);
            return new JsonResult(hotel);
        }

        public async Task<JsonResult> OnPostCreateAjaxAsync([FromForm] CreateHotelDTO input)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });

            var created = await _hotelService.CreateAsync(input);
            return new JsonResult(new { success = true, hotel = created });
        }

        public async Task<JsonResult> OnPostEditAjaxAsync([FromForm] UpdateHotelDTO input)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });

            var updated = await _hotelService.UpdateAsync(input);
            return new JsonResult(new { success = true, hotel = updated });
        }

        public async Task<JsonResult> OnPostDeleteAjaxAsync(Guid id)
        {
            await _hotelService.DeleteAsync(id);
            return new JsonResult(new { success = true });
        }
    }
}
