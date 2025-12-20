using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SanController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public SanController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        [Route("/San/List")]
        public IActionResult GetAllSan()
        {
            var result = qly.Sans
                .Select(s => new SanDto
                {
                    MaSan = s.MaSan,
                    TenSan = s.TenSan,
                    KieuSan = s.KieuSan,
                    TrangThai = s.TrangThai,
                    ViTri = s.ViTri,
                    HinhAnh = s.HinhAnh,
                    Gia = s.Gia
                })
                .ToList();

            return Ok(result);
        }

        // GET: api/San/tenSan
        [HttpGet]
        [Route("/TenSan/{tenSan}")]
        public IActionResult GetSanByTen(string tenSan)
        {
            var result = qly.Sans
                .Where(s => s.TenSan.Contains(tenSan))
                .Select(s => new SanDto
                {
                    MaSan = s.MaSan,
                    TenSan = s.TenSan,
                    KieuSan = s.KieuSan,
                    TrangThai = s.TrangThai,
                    ViTri = s.ViTri
                })
                .ToList();

            if (result == null || !result.Any())
            {
                return NotFound("Không tìm thấy sân.");
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("/San/Insert")]
        public IActionResult Insert([FromBody] SanDto newSan)
        {
            if(newSan == null)
            {
                return BadRequest("Dữ liệu sản phẩm không hợp lệ");
            }

            int maxMaSan = 0;
            if (qly.Sans.Any())
            {
                maxMaSan = qly.Sans.Max(x => x.MaSan);
            }

            var etity = new San
            {
                MaSan = maxMaSan + 1,
                TenSan = newSan.TenSan,
                KieuSan = newSan.KieuSan,
                TrangThai = newSan.TrangThai,
                ViTri = newSan.ViTri,
                HinhAnh = newSan.HinhAnh,
                Gia = newSan.Gia
            };
            qly.Sans.Add(etity);
            qly.SaveChanges();
            return Ok(etity);
        }
        [HttpPut]
        [Route("/San/Update/{maSan}")]
        public IActionResult Update(int maSan, [FromBody] SanDto newSan)
        {
            var sanCanUpdate = qly.Sans.FirstOrDefault(s => s.MaSan == maSan);
            if(sanCanUpdate == null)
            {
                return BadRequest($"Không tìm thấy sân có mã = {maSan}");
            }
            //Cập nhật sân
            sanCanUpdate.MaSan = maSan;
            sanCanUpdate.TenSan = newSan.TenSan ?? sanCanUpdate.TenSan;
            sanCanUpdate.KieuSan = newSan.KieuSan ?? sanCanUpdate.KieuSan;
            sanCanUpdate.TrangThai = newSan.TrangThai ?? sanCanUpdate.TrangThai;
            sanCanUpdate.ViTri = newSan.ViTri ?? sanCanUpdate.ViTri;
            sanCanUpdate.HinhAnh = newSan.HinhAnh ?? sanCanUpdate.HinhAnh;
            sanCanUpdate.Gia = newSan.Gia ?? sanCanUpdate.Gia;

            qly.SaveChanges();
            return (Ok("Cập nhật thành công"));
        }

        [HttpDelete]
        [Route("/San/Delete/{maSan}")]
        public IActionResult Delete(int maSan)
        {
            var item = qly.Sans.FirstOrDefault(san => san.MaSan == maSan);

            if(item == null)
            {
                return BadRequest($"Không tìm thấy sân có mã = {maSan}");
            }

            qly.Sans.Remove(item);
            qly.SaveChanges();
            return Ok($"Đã xóa sân với mã = {maSan}");
        }
    }
}
