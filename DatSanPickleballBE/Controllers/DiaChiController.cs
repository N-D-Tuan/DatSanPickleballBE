using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DiaChiController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public DiaChiController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        // GET diachi/thanhpho
        [HttpGet("thanhpho")]
        public async Task<ActionResult<IEnumerable<ThanhPhoDto>>> GetThanhPho()
        {
            var data = await qly.ThanhPhos
                .Select(tp => new ThanhPhoDto
                {
                    MaTp = tp.MaTp,
                    TenTp = tp.TenTp
                })
                .ToListAsync();

            return Ok(data);
        }

        // GET diachi/quan/{maTP}
        [HttpGet("quan/{maTP}")]
        public async Task<ActionResult<IEnumerable<QuanHuyenDto>>> GetQuanByThanhPho(int maTP)
        {
            var data = await qly.QuanHuyens
                .Where(q => q.MaTp == maTP)
                .Select(q => new QuanHuyenDto
                {
                    MaQh = q.MaQh,
                    TenQh = q.TenQh,
                    MaTp = q.MaTp
                })
                .ToListAsync();

            return Ok(data);
        }
    }
}
