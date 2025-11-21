using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.RoomDto.RoomTypeDTO
{
    public class CreateRoomTypeDTO
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
