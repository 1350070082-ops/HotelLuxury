using Microsoft.AspNetCore.Mvc;
using HotelLuxuryWeb.Controllers;

namespace HotelLuxuryWeb.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout(string roomName, string price)
        {
            ViewBag.RoomName = roomName;
            ViewBag.Price = price;
            return View();
        }

        [HttpPost]
        public IActionResult Confirm(string CustomerName, string Phone, string RoomName, string Price, string CheckInDate, string CheckOutDate, string PaymentMethod)
        {
            // --- BƯỚC QUAN TRỌNG: XÓA ĐOẠN AdminController._customers.Add Ở ĐÂY ---
            // Trâm đừng lưu dữ liệu tại đây nữa, vì trang Confirm.cshtml sẽ làm việc này 
            // thông qua form tự động gửi về AdminController rồi.

            // 1. Chỉ truyền dữ liệu sang ViewBag để trang Confirm hiển thị và gửi đi
            ViewBag.CustomerName = CustomerName;
            ViewBag.Phone = Phone;
            ViewBag.RoomName = RoomName;
            ViewBag.Price = Price; // Giữ nguyên chuỗi để trang Confirm xử lý
            ViewBag.CheckInDate = CheckInDate;
            ViewBag.CheckOutDate = CheckOutDate;
            ViewBag.PaymentMethod = PaymentMethod;

            // 2. Trả về View Confirm (Trang có dấu tích xanh và form tự động)
            return View(); 
        }
    }
}