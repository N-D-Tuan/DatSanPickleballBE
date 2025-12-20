using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers.shop
{
    [Route("[controller]")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public GioHangController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        [Route("/GioHang/ListAll")]
        public IActionResult GetList()
        {
            var result = from gh in qly.GioHangs
                         select new GioHangDto
                         {
                             MaNguoiDung = gh.MaNguoiDung,
                             MaSanPham = gh.MaSanPham,
                             SoLuong = gh.SoLuong
                         };
            return Ok(result);
        }

        [HttpGet]
        [Route("/GioHang/MaNguoiDung/{maNguoiDung}")]
        public IActionResult GetGioHangByMaNguoiDung(int maNguoiDung)
        {
            var result = qly.GioHangs
                .Where(gh => gh.MaNguoiDung == maNguoiDung)
                .Include(gh => gh.MaSanPhamNavigation)
                    .ThenInclude(sp => sp.MaDanhMucNavigation)
                .Include(gh => gh.MaSanPhamNavigation)
                    .ThenInclude(sp => sp.MaGiamGia)
                .AsEnumerable()
                .Select(gh =>
                {
                    var giamGiaSoTien = gh.MaSanPhamNavigation.MaGiamGia
                        .Where(gg => gg.LoaiGiamGia == "sotien" && gg.TrangThai == "HoatDong")
                        .Select(gg => gg.GiaTri)
                        .DefaultIfEmpty(0)
                        .Max();

                    var giamGiaPhanTram = gh.MaSanPhamNavigation.MaGiamGia
                        .Where(gg => gg.LoaiGiamGia == "phantram" && gg.TrangThai == "HoatDong")
                        .Select(gg => gg.GiaTri)
                        .DefaultIfEmpty(0)
                        .Max();

                    var giaBanSauTien = gh.MaSanPhamNavigation.GiaBan
                                        - giamGiaSoTien
                                        - (gh.MaSanPhamNavigation.GiaBan * giamGiaPhanTram / 100);

                    if (giaBanSauTien < 0) giaBanSauTien = 0;

                    return new GioHangDto
                    {
                        MaNguoiDung = gh.MaNguoiDung,
                        MaSanPham = gh.MaSanPham,
                        SoLuong = gh.SoLuong,
                        TenSanPham = gh.MaSanPhamNavigation.TenSanPham,
                        MoTa = gh.MaSanPhamNavigation.MoTa,
                        GiaBan = giaBanSauTien,
                        HinhAnh = gh.MaSanPhamNavigation.HinhAnh,
                        MaDanhMuc = gh.MaSanPhamNavigation.MaDanhMuc,
                        SoLuongTon = gh.MaSanPhamNavigation.SoLuongTon,
                        TenDanhMuc = gh.MaSanPhamNavigation.MaDanhMucNavigation.TenDanhMuc
                    };

                })
                .ToList();

            return Ok(result);
        }

        [HttpPost]
        [Route("/GioHang/Insert")]
        public IActionResult Insert([FromBody] GioHangInsertDto newGioHang)
        {
            if(newGioHang == null)
            {
                return BadRequest("thông tin không hợp lệ");
            }
            //tìm xem có chưa
            var entities = qly.GioHangs
                .FirstOrDefault(g => g.MaNguoiDung == newGioHang.MaNguoiDung && g.MaSanPham == newGioHang.MaSanPham);
            if(entities != null)
            {
                entities.SoLuong += newGioHang.SoLuong;
            } else
            {
                var entity = new GioHang()
                {
                    MaNguoiDung = newGioHang.MaNguoiDung,
                    MaSanPham = newGioHang.MaSanPham,
                    SoLuong = newGioHang.SoLuong
                };
                qly.GioHangs.Add(entity);
            }
           
            qly.SaveChanges();
            return Ok(newGioHang);
        }

        //delete 1
        [HttpDelete]
        [Route("/GioHang/Delete")]
        public IActionResult Delete([FromBody] GioHangInsertDto ghDelete)
        {
            if (ghDelete == null)
            {
                return BadRequest("Thông tin không hợp lệ");
            }

            var entity = qly.GioHangs
                .FirstOrDefault(gh => gh.MaNguoiDung == ghDelete.MaNguoiDung
                                    && gh.MaSanPham == ghDelete.MaSanPham );

            if (entity == null)
            {
                return BadRequest("Không tồn tại sản phẩm này");
            }
            
            qly.GioHangs.Remove(entity);
            qly.SaveChanges();
            return Ok(new GioHangDto
            {
                MaNguoiDung = entity.MaNguoiDung,
                MaSanPham = entity.MaSanPham,
                SoLuong = entity.SoLuong
            });
        }
        //delete all
        [HttpDelete]
        [Route("/GioHang/DeleteAll/{maNguoiDung}")]
        public IActionResult DeleteAll(int maNguoiDung)
        {
            var entity = qly.GioHangs
                .Where(gh => gh.MaNguoiDung == maNguoiDung).ToList();

            if (!entity.Any())
            {
                return NotFound("Giỏ hàng trống");
            }

            qly.GioHangs.RemoveRange(entity);
            qly.SaveChanges();
            return Ok($"Đã xóa toàn bộ giỏ hàng của người dùng {maNguoiDung}");
        }
    }
}
