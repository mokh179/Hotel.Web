using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Locations.City;
using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.DTOs.Pagination;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.User.Pages.Hotels
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IHotelService _hotelService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;

        public IndexModel(
            IHotelService hotelService,
            ICountryService countryService,
            ICityService cityService)
        {
            _hotelService = hotelService;
            _countryService = countryService;
            _cityService = cityService;
        }

        public PagedResult<HotelDTO> Result { get; set; }

        public List<CountryDTO> Countries { get; set; }
        public List<CityDTO> Cities { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid? CountryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid? CityId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        public async Task OnGet()
        {
            Countries = await _countryService.GetAllAsync();

            if (CountryId.HasValue)
                Cities = await _cityService.GetByCountryAsync(CountryId.Value);
            else
                Cities = new List<CityDTO>();

            Result = await _hotelService.SearchHotelsAsync(
               SearchName,
               CountryId,
               CityId,
               Page,
               pageSize: 6
           );
        }

        public async Task<JsonResult> OnGetCities(Guid countryId)
        {
            var cities = await _cityService.GetByCountryAsync(countryId);
            return new JsonResult(cities);
        }
    }
}
