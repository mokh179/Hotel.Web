using Hotel.Application.DTOs.Locations.Country;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.HotelDto
{
    public class CreateHotelDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public Guid CityId { get; set; }
        [Required]
        public Guid CountryId { get; set; }

        public string? Description { get; set; } 
        public string? PhoneNumber { get; set; } 
        public string? Email { get; set; } 
        public double Rating { get; set; }
        public List<CountryDTO> Countries { get; set; } = new();

    }
}
