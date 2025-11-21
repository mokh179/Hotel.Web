using Hotel.Application.DTOs.Locations.Country;
using Hotel.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel.Web.Areas.Admin.Pages.Locations
{
    public class CountriesModel : PageModel
    {
        private readonly ICountryService _countryService;

        public CountriesModel(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public List<CountryDTO> Countries { get; set; } = new();

        public async Task OnGetAsync()
        {
            Countries = await _countryService.GetAllAsync();
        }

        // GET: Load single country
        public async Task<JsonResult> OnGetCountryAsync(Guid id)
        {
            var c = await _countryService.GetByIdAsync(id);
            return new JsonResult(c);
        }

        // POST: Create country
        public async Task<JsonResult> OnPostCreateAjaxAsync([FromForm] CreateCountryDTO input)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var created = await _countryService.CreateAsync(input);
                return new JsonResult(new { success = true, country = created });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }

        // POST: Edit country
        public async Task<JsonResult> OnPostEditAjaxAsync([FromForm] UpdateCountryDTO input)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var updated = await _countryService.UpdateAsync(input);
                if (updated == null)
                    return new JsonResult(new { success = false, errors = new[] { "Country not found" } });

                return new JsonResult(new { success = true, country = updated });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, errors = new[] { ex.Message } });
            }
        }

        // POST: Delete
        public async Task<JsonResult> OnPostDeleteAjaxAsync(Guid id)
        {
            var success = await _countryService.DeleteAsync(id);
            return new JsonResult(new { success });
        }
    }
}
