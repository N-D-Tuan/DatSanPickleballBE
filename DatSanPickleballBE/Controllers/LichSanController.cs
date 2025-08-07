using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichSanController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public LichSanController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LichSanDto>>> GetAllLichSan()
        {
            var list = await qly.LichSans
                .Select(ls => new LichSanDto
                {
                    MaLichSan = ls.MaLichSan,
                    MaSan = ls.MaSan.GetValueOrDefault(),
                    MaKhungGio = ls.MaKhungGio.GetValueOrDefault(),
                    Ngay = ls.Ngay.GetValueOrDefault(),
                    IsBooked = ls.IsBooked.GetValueOrDefault()
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("search-by-id/{maLichSan}")]
        public async Task<ActionResult<LichSanDto>> GetLichSanById(int maLichSan)
        {
            var ls = await qly.LichSans.FirstOrDefaultAsync(l => l.MaLichSan == maLichSan);

            if (ls == null)
            {
                return NotFound();
            }

            var lichSanDto = new LichSanDto
            {
                MaLichSan = ls.MaLichSan,
                MaSan = ls.MaSan.GetValueOrDefault(),
                MaKhungGio = ls.MaKhungGio.GetValueOrDefault(),
                Ngay = ls.Ngay.GetValueOrDefault(),
                IsBooked = ls.IsBooked.GetValueOrDefault()
            };

            return Ok(lichSanDto);
        }
    }
}
