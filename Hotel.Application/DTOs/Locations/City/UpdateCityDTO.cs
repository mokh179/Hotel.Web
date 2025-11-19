using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.Locations.City
{
    public class UpdateCityDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CountryId { get; set; }
    }

}
