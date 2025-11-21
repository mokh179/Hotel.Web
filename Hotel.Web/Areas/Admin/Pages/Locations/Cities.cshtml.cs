using Hotel.Application.DTOs.Locations.City;
using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.Locations
{
    public class CitiesModel : PageModel
    {
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;
        public CitiesModel(ICityService cityService, ICountryService countryService)
        {
            _cityService = cityService;
            _countryService = countryService;
        }

        public List<CityDTO> Cities { get; set; } = new();
        public List<CountryDTO> Countries { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public Guid? CountryId { get; set; }
        public async Task OnGetAsync()
        {
            Countries = await _countryService.GetAllAsync();
            Cities = CountryId.HasValue
                ? await _cityService.GetByCountryAsync(CountryId.Value)
                : await _cityService.GetAllAsync();
        }

        public async Task<JsonResult> OnGetCityAsync(Guid id)
        {
            var city = await _cityService.GetByIdAsync(id);
            return new JsonResult(city);
        }

        public async Task<JsonResult> OnPostCreateAjaxAsync([FromForm] CreateCityDTO input)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            try
            {
                var created = await _cityService.CreateAsync(input);
                return new JsonResult(new { success = true, city = created });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }

        public async Task<JsonResult> OnPostEditAjaxAsync([FromForm] UpdateCityDTO input)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

            try
            {
                var updated = await _cityService.UpdateAsync(input);
                if (updated == null)
                    return new JsonResult(new { success = false, errors = new[] { "City not found" } });

                return new JsonResult(new { success = true, city = updated });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }

        public async Task<JsonResult> OnPostDeleteAjaxAsync(Guid id)
        {
            var ok = await _cityService.DeleteAsync(id);
            return new JsonResult(new { success = ok });
        }
    }
}
