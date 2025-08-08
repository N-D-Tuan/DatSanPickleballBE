using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LichSanController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public LichSanController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        [Route("/LichSan/ListLichSan")]
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

        [HttpGet]
        [Route("/LichSan/ListNgay")]
        public IActionResult GetNgay()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var ngayList = qly.LichSans
                .Where(ls => ls.Ngay >= today)
                .Select(ls => ls.Ngay) // Lấy phần ngày, bỏ giờ
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            if (ngayList == null || !ngayList.Any())
            {
                return NotFound(new { message = "Không có ngày nào phù hợp." });
            }

            return Ok(ngayList);
        }

        [HttpGet]
        [Route("/MaLichSan/{maLichSan}")]
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
