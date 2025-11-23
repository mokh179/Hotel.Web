using AutoMapper;
using Hotel.Application.DTOs.Locations.City;
using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.Locations
{
    [Authorize(Roles = "Admin")]

    [IgnoreAntiforgeryToken]
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
        public Guid? CountryId { get; set; }

        public async Task OnGetAsync(Guid? countryId)
        {
            CountryId = countryId;

            Countries = await _countryService.GetAllAsync();

            Cities = countryId.HasValue
                ? await _cityService.GetByCountryAsync(countryId.Value)
                : await _cityService.GetAllAsync();
        }

        // -------- GET ONE --------
        public async Task<JsonResult> OnGetCityAsync(Guid id)
        {
            var city = await _cityService.GetByIdAsync(id);
            return new JsonResult(city);
        }

        // -------- CREATE --------
        public async Task<JsonResult> OnPostCreateAjaxAsync([FromForm] CreateCityDTO dto)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var created = await _cityService.CreateAsync(dto);

            // reload to get CountryName
            created = await _cityService.GetByIdAsync(created.Id);

            return new JsonResult(new { success = true, city = created });
        }

        // -------- EDIT --------
        public async Task<JsonResult> OnPostEditAjaxAsync([FromForm] UpdateCityDTO dto)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var updated = await _cityService.UpdateAsync(dto);

            updated = await _cityService.GetByIdAsync(updated.Id);

            return new JsonResult(new { success = true, city = updated });
        }

        // -------- DELETE --------
        public async Task<JsonResult> OnPostDeleteAjaxAsync(Guid id)
        {
            var ok = await _cityService.DeleteAsync(id);
            return new JsonResult(new { success = ok });
        }
    }
}
