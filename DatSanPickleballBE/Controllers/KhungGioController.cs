using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KhungGioController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public KhungGioController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        [Route("/KhungGio/List")]
        public async Task<ActionResult<IEnumerable<KhungGioDto>>> GetAllKhungGio()
        {
            var list = await qly.KhungGios
                .Select(kg => new KhungGioDto
                {
                    MaKhungGio = kg.MaKhungGio,
                    GioBatDau = kg.GioBatDau,
                    GioKetThuc = kg.GioKetThuc
                }).ToListAsync();

            return Ok(list);
        }

        [HttpGet]
        [Route("/KhungGio/{gio}")]
        public async Task<ActionResult<IEnumerable<KhungGioDto>>> GetKhungGioTheoGio([FromQuery] TimeOnly gio)
        {
            var list = await qly.KhungGios
                .Where(kg => kg.GioBatDau == gio || kg.GioKetThuc == gio)
                .Select(kg => new KhungGioDto
                {
                    MaKhungGio = kg.MaKhungGio,
                    GioBatDau = kg.GioBatDau,
                    GioKetThuc = kg.GioKetThuc
                }).ToListAsync();

            if (list.Count == 0)
                return NotFound("Không tìm thấy khung giờ theo thời gian yêu cầu.");

            return Ok(list);
        }
    }
}
