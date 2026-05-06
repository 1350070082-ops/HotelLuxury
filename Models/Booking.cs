using System.ComponentModel.DataAnnotations; // Thêm dòng này để dùng [Key]

namespace HotelLuxuryWeb.Models
{
    public class Booking
    {
        [Key] // Chỉ định đây là khóa chính để khớp với SQL Server
        public int BookingId { get; set; } 

        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? RoomName { get; set; }
        public int Price { get; set; }
        public string? Status { get; set; }
    }
}