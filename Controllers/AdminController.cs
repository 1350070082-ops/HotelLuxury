using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System;
using HotelLuxuryWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelLuxuryWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true") return RedirectToAction("Login");
            
            var bookings = _context.Bookings.ToList();

            ViewBag.TotalOrders = bookings.Count;
            ViewBag.TotalRevenue = bookings.Sum(c => c.Price);
            ViewBag.TotalCustomers = bookings.Select(c => c.CustomerName).Distinct().Count();

            int totalHotelRooms = 20;
            ViewBag.TotalRooms = totalHotelRooms;
            ViewBag.OccupiedRooms = bookings.Count; 
            ViewBag.AvailableRooms = totalHotelRooms - bookings.Count;

            return View(bookings);
        }

        [HttpPost]
        public IActionResult Confirm(string CustomerName, string Phone, string RoomName, string Price)
        {
            int roomPrice = 0;
            int.TryParse(Price, out roomPrice);

            if (roomPrice == 0) 
            {
                if (RoomName.Contains("Deluxe")) roomPrice = 1200000;
                else if (RoomName.Contains("President")) roomPrice = 5000000;
                else roomPrice = 850000;
            }

            string finalRoom = RoomName;
            if (!finalRoom.Contains("#"))
            {
                finalRoom += " #" + new Random().Next(100, 500);
            }

            var newBooking = new Booking { 
                CustomerName = CustomerName, 
                Phone = Phone, 
                RoomName = finalRoom, 
                Status = "Đã thanh toán", 
                Price = roomPrice 
            };

            _context.Bookings.Add(newBooking); 
            _context.SaveChanges(); 

            return RedirectToAction("Index", "Booking");
        }

        [HttpPost]
        public IActionResult Update(int id, string name, string phone, int price, string status)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                booking.CustomerName = name;
                booking.Phone = phone;
                booking.Price = price;
                booking.Status = status;

                _context.SaveChanges(); 
            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Delete(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges(); 
            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Edit(int id)
        {
            var customer = _context.Bookings.Find(id);
            if (customer == null) return RedirectToAction("Dashboard");
            return View(customer);
        }

        // --- PHẦN ĐĂNG NHẬP ĐÃ CHỈNH SỬA ĐỂ LẤY TỪ DATABASE ---
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Tìm tài khoản trong bảng Users mà Trâm vừa tạo
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                // Nếu tìm thấy tài khoản hợp lệ trong SQL
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Dashboard");
            }

            // Nếu không tìm thấy hoặc sai thông tin
            ViewBag.Error = "Tài khoản hoặc mật khẩu trong database không đúng!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}