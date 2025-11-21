using Hotel.Application.DTOs.BookingDto;
using Hotel.Application.DTOs.HotelDto;
using Hotel.Application.DTOs.RoomDto;
using Hotel.Application.DTOs.UserProfileDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.DTOs.Admin
{
    public class StatisticsDTO
    {
        public List<HotelDTO> Hotels { get; set; }
        public List<RoomDTO> Rooms { get; set; }
        public List<BookingDTO> Bookings { get; set; }
        public List<UserDTO> Users { get; set; }


        public int TotalHotels { get; set; }
        public int TotalRooms { get; set; }
        public int TotalBookings { get; set; }
        public int TotalUsers { get; set; }

        public int TodayBookings { get; set; }
        public int FutureBookings { get; set; }
        public int PastBookings { get; set; }
        public string TopHotelName { get; set; }
        public string MostBookedRoomType { get; set; }
        public decimal TotalRevenue { get; set; }
        public double AverageStayLength { get; set; }

        public List<BookingDTO> RecentBookings { get; set; }
        public List<UserDTO> RecentUsers { get; set; }
    }
}
