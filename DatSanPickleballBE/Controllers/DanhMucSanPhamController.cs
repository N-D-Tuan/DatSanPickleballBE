using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatSanPickleballBE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DanhMucSanPhamController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public DanhMucSanPhamController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        [Route("/DanhMucSanPham/ListDanhMuc")]
        public IActionResult GetAll()
        {
            var result = qly.DanhMucSanPhams
                .Select(s => new DanhMucSanPhamDto
                {
                    MaDanhMuc = s.MaDanhMuc,
                    TenDanhMuc = s.TenDanhMuc,
                    MoTa = s.MoTa
                });
            return Ok(result);
        }

        [HttpPost]
        [Route("/DanhMucSanPham/Insert")]
        public IActionResult InsertDanhMuc([FromBody] DanhMucSanPhamDto newDanhMuc)
        {
            if(newDanhMuc == null)
            {
                return BadRequest("Dữ liệu danh mục không hợp lệ");
            }

            var entity = new DanhMucSanPham
            {
                MaDanhMuc = newDanhMuc.MaDanhMuc,
                TenDanhMuc = newDanhMuc.TenDanhMuc,
                MoTa = newDanhMuc.MoTa
            };

            qly.DanhMucSanPhams.Add(entity);
            qly.SaveChanges();
            return Ok(entity);
        }
    }
}
