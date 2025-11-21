using Hotel.Application.DTOs.Locations.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services
{
    public interface ICityService
    {
        Task<List<CityDTO>> GetAllAsync();
        Task<CityDTO> GetByIdAsync(Guid id);
        Task<CityDTO> CreateAsync(CreateCityDTO dto);
        Task<CityDTO> UpdateAsync(UpdateCityDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<CityDTO>> GetByCountryAsync(Guid countryId); 
    }
}
