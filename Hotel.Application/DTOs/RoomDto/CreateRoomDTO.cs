using Hotel.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.RoomDto
{
    public class CreateRoomDTO
    {
        [Required(ErrorMessage = "Room number is required.")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Room number must be between 1 and 10 characters.")]
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; } = default!;

        [Required(ErrorMessage = "Room type is required.")]
        [Display(Name = "Room Type")]
        public Guid RoomTypeId { get; set; }    // renamed from RoomType -> RoomTypeId

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Hotel is required.")]
        [Display(Name = "Hotel")]
        public Guid HotelId { get; set; }

        // optional description / notes
        [StringLength(1000, ErrorMessage = "Description can't be longer than 1000 characters.")]
        public string? Description { get; set; }
    }

}
