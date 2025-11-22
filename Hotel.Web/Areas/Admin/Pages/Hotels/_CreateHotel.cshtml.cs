using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.Interfaces.Services;
using Hotel.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.Hotels
{
    public class _CreateHotelModel : PageModel
    {
        private readonly IHotelService _hotelService;
        private readonly ICountryService _countryService;

        public _CreateHotelModel(IHotelService hotelService, ICountryService countryService)
        {
            _hotelService = hotelService;
            _countryService = countryService;
        }

        public List<CountryDTO> Countries { get; set; } = new();
        

        public async Task OnGetAsync()
        {
            Countries = await _countryService.GetAllAsync();

            // load list of countries for dropdown (use your units/repo that returns countries)
            // Example: Countries = await _hotelService.GetCountriesAsync();
            // For now keep empty; fill in based on your existing service for locations
        }
    }
}
