using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public SanController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        public IActionResult GetAllSan()
        {
            var result = qly.Sans
                .Select(s => new SanDto
                {
                    MaSan = s.MaSan,
                    TenSan = s.TenSan,
                    KieuSan = s.KieuSan,
                    TrangThai = s.TrangThai,
                    ViTri = s.ViTri
                })
                .ToList();

            return Ok(result);
        }

        // GET: api/San/tenSan
        [HttpGet("search-by-name/{tenSan}")]
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
    }
}
