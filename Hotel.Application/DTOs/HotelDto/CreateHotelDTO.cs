using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.HotelDto
{
    public class CreateHotelDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public Guid CityId { get; set; }

        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double Rating { get; set; }

    }
}
