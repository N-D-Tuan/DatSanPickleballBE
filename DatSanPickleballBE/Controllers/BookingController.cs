using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace DatSanPickleballBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;
        public BookingController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings()
        {
            var bookings = await qly.Bookings
                .Select(b => new BookingDto
                {
                    MaBooking = b.MaBooking,
                    MaNguoiDung = b.MaNguoiDung.GetValueOrDefault(),
                    TrangThai = b.TrangThai,
                    MaLichSan = b.MaLichSan.GetValueOrDefault()
                })
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("nguoidung/{maNguoiDung}")]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookingsByNguoiDung(int maNguoiDung)
        {
            var list = await qly.Bookings
                .Where(b => b.MaNguoiDung == maNguoiDung)
                .Select(b => new BookingDto
                {
                    MaBooking = b.MaBooking,
                    MaNguoiDung = b.MaNguoiDung.GetValueOrDefault(),
                    TrangThai = b.TrangThai,
                    MaLichSan = b.MaLichSan.GetValueOrDefault()
                })
                .ToListAsync();

            if (list == null || list.Count == 0)
                return NotFound("Không tìm thấy Booking nào của người dùng này.");

            return Ok(list);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto bookingInput)
        {
            try
            {
                if (bookingInput == null || bookingInput.MaNguoiDung == 0 || bookingInput.MaLichSan == 0)
                {
                    return BadRequest("Thiếu thông tin maNguoiDung hoặc maLichSan.");
                }

                // Mặc định trạng thái
                string trangThai = "Đã thanh toán";

                // Tạo câu lệnh SQL
                string sql = @"
            INSERT INTO Booking (MaNguoiDung, MaLichSan, TrangThai)
            VALUES (@MaNguoiDung, @MaLichSan, @TrangThai)
        ";

                int rows = await qly.Database.ExecuteSqlRawAsync(sql,
                    new SqlParameter("@MaNguoiDung", bookingInput.MaNguoiDung),
                    new SqlParameter("@MaLichSan", bookingInput.MaLichSan),
                    new SqlParameter("@TrangThai", trangThai)
                );

                if (rows > 0)
                    return Ok("Đã thêm booking thành công.");
                else
                    return StatusCode(500, "Thêm booking thất bại.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        [HttpPut("{maBooking}")]
        public async Task<IActionResult> UpdateBooking(int maBooking, [FromBody] BookingDto updatedBooking)
        {
            try
            {
                if (updatedBooking == null)
                    return BadRequest("Thông tin cập nhật không hợp lệ.");

                // Kiểm tra Booking có tồn tại không
                var existingBooking = await qly.Bookings.FindAsync(maBooking);
                if (existingBooking == null)
                    return NotFound("Không tìm thấy Booking cần cập nhật.");

                // Tạo câu truy vấn SQL thủ công để tránh lỗi trigger
                string sql = @"
UPDATE Booking
SET MaNguoiDung = @MaNguoiDung,
    MaLichSan = @MaLichSan,
    TrangThai = @TrangThai
WHERE MaBooking = @MaBooking";

                // Thực thi truy vấn với tham số
                int rows = await qly.Database.ExecuteSqlRawAsync(sql,
                    new SqlParameter("@MaNguoiDung", updatedBooking.MaNguoiDung),
                    new SqlParameter("@MaLichSan", updatedBooking.MaLichSan),
                    new SqlParameter("@TrangThai", updatedBooking.TrangThai ?? (object)DBNull.Value),
                    new SqlParameter("@MaBooking", maBooking)
                );

                if (rows > 0)
                    return Ok("Cập nhật Booking thành công.");
                else
                    return StatusCode(500, "Cập nhật Booking thất bại.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }


    }
}