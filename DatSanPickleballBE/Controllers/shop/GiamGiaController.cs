using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers.shop
{
    [Route("[controller]")]
    [ApiController]
    public class GiamGiaController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public GiamGiaController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        [Route("/GiamGia/List")]
        public IActionResult Get()
        {
            var result = qly.GiamGia.Select(gg => new GiamGiaDto
            {
                MaGiamGia = gg.MaGiamGia,
                MaCode = gg.MaCode,
                MoTa = gg.MoTa,
                LoaiGiamGia = gg.LoaiGiamGia,
                GiaTri = gg.GiaTri,
                NgayBatDau = gg.NgayBatDau,
                NgayKetThuc = gg.NgayKetThuc,
                SoLanSuDungMax = gg.SoLanSuDungMax,
                SoLanDaSuDung = gg.SoLanDaSuDung,
                TrangThai = gg.TrangThai,
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("/GiamGia/Insert")]
        public IActionResult Insert([FromBody] GiamGiaDto newGiamGia)
        {
            if(newGiamGia == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }
            try
            {
                var checkGiamGiam = qly.GiamGia.FirstOrDefault(gg => gg.MaCode == newGiamGia.MaCode);
                if (checkGiamGiam != null)
                {
                    return BadRequest("Mã code này đã tồi tại");
                }
                var entity = new GiamGia
                {
                    MaCode = newGiamGia.MaCode,
                    MoTa = newGiamGia.MoTa,
                    LoaiGiamGia = newGiamGia.LoaiGiamGia,
                    GiaTri = newGiamGia.GiaTri,
                    NgayBatDau = newGiamGia.NgayBatDau,
                    NgayKetThuc = newGiamGia.NgayKetThuc,
                    SoLanSuDungMax = newGiamGia.SoLanSuDungMax
                };
                qly.GiamGia.Add(entity);
                qly.SaveChanges();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Có lỗi xảy ra trong quá trình thêm giảm giá");
            }
            
        }
        [HttpPut]
        [Route("/GiamGia/Update/{maGiamGia}")]
        public IActionResult Update(int maGiamGia, [FromBody] GiamGiaDto newGiamGia)
        {
            try
            {
                var giamGiaCanUpdate = qly.GiamGia.FirstOrDefault(gg => gg.MaGiamGia == maGiamGia);
                if (giamGiaCanUpdate == null)
                {
                    return BadRequest("Không tìm thấy giảm giá này");
                }

                if (giamGiaCanUpdate.MaCode != newGiamGia.MaCode)
                {
                    var checkGiamGiam = qly.GiamGia.FirstOrDefault(gg => gg.MaCode == newGiamGia.MaCode);
                    if (checkGiamGiam != null)
                    {
                        return BadRequest("Mã code này đã tồi tại");
                    }
                }

                giamGiaCanUpdate.MaCode = newGiamGia.MaCode ?? giamGiaCanUpdate.MaCode;
                giamGiaCanUpdate.MoTa = newGiamGia.MoTa ?? giamGiaCanUpdate.MoTa;
                giamGiaCanUpdate.LoaiGiamGia = newGiamGia.LoaiGiamGia ?? giamGiaCanUpdate.LoaiGiamGia;
                giamGiaCanUpdate.GiaTri = newGiamGia.GiaTri ?? giamGiaCanUpdate.GiaTri;
                giamGiaCanUpdate.NgayBatDau = newGiamGia.NgayBatDau ?? giamGiaCanUpdate.NgayBatDau;
                giamGiaCanUpdate.NgayKetThuc = newGiamGia.NgayKetThuc ?? giamGiaCanUpdate.NgayKetThuc;
                giamGiaCanUpdate.SoLanSuDungMax = newGiamGia.SoLanSuDungMax ?? giamGiaCanUpdate.SoLanSuDungMax;
                qly.SaveChanges();
                return Ok(giamGiaCanUpdate);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Có lỗi xảy ra trong quá trình sửa giảm giá");
            }
            
        }

        [HttpDelete]
        [Route("/GiamGia/Delete/{maGiamGia}")]
        public IActionResult Delete(int maGiamGia)
        {
            var giamGiaCanUpdate = qly.GiamGia.FirstOrDefault(gg => gg.MaGiamGia == maGiamGia);
            if (giamGiaCanUpdate == null)
            {
                return BadRequest("Không tìm thấy giảm giá này");
            }

            qly.GiamGia.Remove(giamGiaCanUpdate);
            qly.SaveChanges();
            return Ok("đã xóa"+giamGiaCanUpdate);
        }

        [HttpPut]
        [Route("/GiamGia/Update/TrangThai/{maGiamGia}/{trangThai}")]
        public IActionResult UpdateTrangThai(int maGiamGia, string trangThai)
        {
            try
            {
                var giamGiaCanUpdate = qly.GiamGia.FirstOrDefault(gg => gg.MaGiamGia == maGiamGia);
                if (giamGiaCanUpdate == null)
                {
                    return BadRequest("Không tìm thấy giảm giá này");
                }

                giamGiaCanUpdate.TrangThai = trangThai;

                qly.SaveChanges();
                return Ok(giamGiaCanUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Có lỗi xảy ra trong quá trình sửa giảm giá");
            }

        }

        [HttpPost]
        [Route("/GiamGia/ApDung")]
        public IActionResult ApDungMaGiamGia([FromBody] ApDungGiamGiaRequest request)
        {
            var giamGia = qly.GiamGia.Include(g => g.MaSanPhams).FirstOrDefault(g => g.MaGiamGia == request.MaGiamGia);
            if(giamGia == null)
            {
                return NotFound("Không tìm thấy mã giảm giá");
            }
            var sanPhams = qly.SanPhams.Where(sp => request.DanhSachSanPham.Contains(sp.MaSanPham)).ToList();
            foreach (var sp in sanPhams)
            {
                if(!giamGia.MaSanPhams.Any(x => x.MaSanPham == sp.MaSanPham))
                {
                    giamGia.MaSanPhams.Add(sp);
                }
            }
            qly.SaveChanges();
            return Ok("Áp dụng thành công");
        }

        [HttpDelete]
        [Route("/GiamGia/DeleteApDung")]
        public IActionResult DeleteApDung([FromBody] ApDungGiamGiaRequest request)
        {
            var giamGia = qly.GiamGia.Include(g => g.MaSanPhams).FirstOrDefault(g => g.MaGiamGia == request.MaGiamGia);
            if (giamGia == null)
            {
                return NotFound("Không tìm thấy mã giảm giá");
            }

            // Lấy danh sách sản phẩm cần hủy
            var sanPhams = qly.SanPhams
                .Where(sp => request.DanhSachSanPham.Contains(sp.MaSanPham))
                .ToList();

            foreach (var sp in sanPhams)
            {
                if (giamGia.MaSanPhams.Any(x => x.MaSanPham == sp.MaSanPham))
                {
                    giamGia.MaSanPhams.Remove(sp);
                }
            }

            qly.SaveChanges();
            return Ok("Đã hủy áp dụng mã giảm giá cho sản phẩm");
        }

    }
}
