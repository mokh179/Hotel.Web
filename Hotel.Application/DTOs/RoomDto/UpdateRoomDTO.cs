using Hotel.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.RoomDto
{
    public class UpdateRoomDTO : CreateRoomDTO
    {
        [Required]
        public Guid Id { get; set; }
    }

}
