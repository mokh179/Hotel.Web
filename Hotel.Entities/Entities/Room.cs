using Hotel.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Entities.Entities
{
    public class Room : BaseEntity
    {
        [Required]
        [StringLength(10)]
        public string RoomNumber { get; set; }

        [Required]
        public RoomType RoomType { get; set; }

        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Required]
        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
