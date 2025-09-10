using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatSanPickleballBE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public DonHangController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateDonHang([FromBody] CreateDonHangDto model)
        {
            if (model == null) return BadRequest("Dữ liệu không hợp lệ!");
            if (model.TongTien < 0) return BadRequest("tongTien phải >= 0");
            if (model.ChiTietDonHangs == null || model.ChiTietDonHangs.Count == 0)
                return BadRequest("Cần ít nhất 1 chi tiết đơn hàng");

            var donHang = new DonHang
            {
                MaNguoiDung = model.MaNguoiDung,
                NgayDat = DateTime.Now,     
                TrangThai = "Chờ xử lý",        
                TongTien = model.TongTien,        
                MaTP = model.MaTP,
                MaQH = model.MaQH,
                DiaChi = model.DiaChi
            };

            qly.DonHangs.Add(donHang);
            await qly.SaveChangesAsync();

            // Thêm chi tiết đơn hàng (không tính lại tổng)
            var details = model.ChiTietDonHangs.Select(ct => new ChiTietDonHang
            {
                MaDonHang = donHang.MaDonHang,
                MaSanPham = ct.MaSanPham,
                SoLuong = ct.SoLuong,
                DonGia = ct.DonGia
            });

            await qly.ChiTietDonHangs.AddRangeAsync(details);
            await qly.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tạo đơn hàng thành công",
                MaDonHang = donHang.MaDonHang,
                NgayDat = donHang.NgayDat,
                TrangThai = donHang.TrangThai,
                TongTien = donHang.TongTien
            });
        }
    }
}
