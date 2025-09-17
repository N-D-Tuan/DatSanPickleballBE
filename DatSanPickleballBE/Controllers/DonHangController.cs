using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet("nguoidung/{maNguoiDung}")]
        public async Task<ActionResult<IEnumerable<DonHangDto>>> GetDonHangByNguoiDung(int maNguoiDung)
        {
            var donHangs = await (from dh in qly.DonHangs
                                  join tp in qly.ThanhPhos on dh.MaTP equals tp.MaTp
                                  join qh in qly.QuanHuyens on dh.MaQH equals qh.MaQh
                                  join nd in qly.Users on dh.MaNguoiDung equals nd.MaNguoiDung
                                  where dh.MaNguoiDung == maNguoiDung
                                  orderby dh.NgayDat descending
                                  select new DonHangDto
                                  {
                                      MaDonHang = dh.MaDonHang,
                                      MaNguoiDung = dh.MaNguoiDung.Value,
                                      TenNguoiDung = nd.TenNguoiDung,
                                      NgayDat = dh.NgayDat.Value,
                                      TongTien = dh.TongTien.Value,
                                      TrangThai = dh.TrangThai,
                                      MaTP = dh.MaTP.Value,
                                      TenTP = tp.TenTp,
                                      MaQH = dh.MaQH.Value,
                                      TenQH = qh.TenQh,
                                      DiaChi = dh.DiaChi,
                                      DonHangDetails = (from ctdh in qly.ChiTietDonHangs
                                                        join sp in qly.SanPhams on ctdh.MaSanPham equals sp.MaSanPham
                                                        where ctdh.MaDonHang == dh.MaDonHang
                                                        select new DonHangDetailDto
                                                        {
                                                            //Nếu muốn tối ưu hơn thì chỉ lấy những trường cần thiết
                                                            MaDonHang = ctdh.MaDonHang,
                                                            MaSanPham = ctdh.MaSanPham,
                                                            TenSanPham = sp.TenSanPham,
                                                            HinhAnh = sp.HinhAnh,
                                                            SoLuong = ctdh.SoLuong.Value,
                                                            DonGia = ctdh.DonGia.Value
                                                        }).ToList()
                                  }).ToListAsync();

            if (donHangs == null || donHangs.Count == 0)
            {
                return NotFound(new { message = "Người dùng chưa có đơn hàng nào" });
            }

            return Ok(donHangs);
        }

        [HttpGet("chitietdonhang/{maDonHang}")]
        public async Task<ActionResult<DonHangDto>> GetDonHangById(int maDonHang)
        {
            var donHang = await (from dh in qly.DonHangs
                                 join tp in qly.ThanhPhos on dh.MaTP equals tp.MaTp
                                 join qh in qly.QuanHuyens on dh.MaQH equals qh.MaQh
                                 where dh.MaDonHang == maDonHang
                                 select new DonHangDto
                                 {
                                     MaDonHang = dh.MaDonHang,
                                     MaNguoiDung = dh.MaNguoiDung.Value,
                                     NgayDat = dh.NgayDat.Value,
                                     TongTien = dh.TongTien.Value,
                                     TrangThai = dh.TrangThai,
                                     MaTP = dh.MaTP.Value,
                                     TenTP = tp.TenTp,
                                     MaQH = dh.MaQH.Value,
                                     TenQH = qh.TenQh,
                                     DiaChi = dh.DiaChi
                                 }).FirstOrDefaultAsync();

            if (donHang == null)
            {
                return NotFound();
            }

            // Lấy chi tiết đơn hàng
            var chiTiet = await (from ctdh in qly.ChiTietDonHangs
                                 join sp in qly.SanPhams on ctdh.MaSanPham equals sp.MaSanPham
                                 where ctdh.MaDonHang == maDonHang
                                 select new DonHangDetailDto
                                 {
                                     MaDonHang = ctdh.MaDonHang,
                                     MaSanPham = ctdh.MaSanPham,
                                     TenSanPham = sp.TenSanPham,
                                     HinhAnh = sp.HinhAnh,
                                     SoLuong = ctdh.SoLuong.Value,
                                     DonGia = ctdh.DonGia.Value
                                     // ThanhTien = auto tính trong DTO
                                 }).ToListAsync();

            donHang.DonHangDetails = chiTiet;

            return Ok(donHang);
        }
        [HttpPut("HuyDonHang/{maDonHang}")]
        public async Task<IActionResult> HuyDonHang(int maDonHang)
        {
            var donHang = await qly.DonHangs.FindAsync(maDonHang);

            if (donHang == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            }

            donHang.TrangThai = "Đã hủy";
            await qly.SaveChangesAsync();

            return Ok(new { message = "Đơn hàng đã được hủy" });
        }
        [HttpPut("VanChuyenDonHang/{maDonHang}")]
        public async Task<IActionResult> VanChuyenDonHang(int maDonHang)
        {
            var donHang = await qly.DonHangs.FindAsync(maDonHang);

            if (donHang == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            }

            donHang.TrangThai = "Đang vận chuyển";
            await qly.SaveChangesAsync();

            return Ok(new { message = "Đơn hàng đang vận chuyển" });
        }
        [HttpPut("GiaoDonHang/{maDonHang}")]
        public async Task<IActionResult> GiaoDonHang(int maDonHang)
        {
            var donHang = await qly.DonHangs.FindAsync(maDonHang);

            if (donHang == null)
            {
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            }

            donHang.TrangThai = "Đã giao";
            await qly.SaveChangesAsync();

            return Ok(new { message = "Đơn hàng đã được giao" });
        }
    }
}
