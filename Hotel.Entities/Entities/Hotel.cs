using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Entities.Entities
{
    public class Hotel : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        public Guid CityId { get; set; }
        public City City { get; set; }

        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public double Rating { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
