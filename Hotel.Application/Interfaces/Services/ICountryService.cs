using Hotel.Application.DTOs.Locations.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces.Services
{
    public interface ICountryService
    {
        Task<List<CountryDTO>> GetAllAsync();
        Task<CountryDTO> GetByIdAsync(Guid id);
        Task<CountryDTO> CreateAsync(CreateCountryDTO dto);
        Task<CountryDTO> UpdateAsync(UpdateCountryDTO dto);
        Task DeleteAsync(Guid id);
    }
}
