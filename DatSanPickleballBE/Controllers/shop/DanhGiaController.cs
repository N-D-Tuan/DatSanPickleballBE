using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatSanPickleballBE.Controllers.shop
{
    [Route("[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public DanhGiaController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        [Route("/DanhGia/ListAll")]
        public IActionResult GetAllReviews()
        {
            var result = from dg in qly.DanhGiaSanPhams
                         join nd in qly.Users on dg.MaNguoiDung equals nd.MaNguoiDung
                         group new { dg, nd } by dg.MaSanPham into g
                         select new
                         {
                             MaSanPham = g.Key,
                             Reviews = g.Select(x => new {
                                 Id = x.dg.MaDanhGia,
                                 UserName = x.nd.TenNguoiDung,
                                 Rating = x.dg.SoSao,
                                 Date = x.dg.NgayDanhGia,
                                 Content = x.dg.BinhLuan
                             }).ToList()
                         };

            return Ok(result.ToList());
        }

        [HttpPost]
        [Route("/DanhGia/Insert")]
        public IActionResult Insert([FromBody] DanhGiaSanPhamDto newDanhGia)
        {
            if (newDanhGia == null)
            {
                return BadRequest("Dữ liệu sản phẩm không hợp lệ");
            }
            if (newDanhGia == null || newDanhGia.MaSanPham <= 0 || newDanhGia.MaNguoiDung <= 0)
            {
                return BadRequest("Thông tin đánh giá không hợp lệ");
            }
            if (newDanhGia.SoSao < 1 || newDanhGia.SoSao > 5)
            {
                return BadRequest("Số sao phải từ 1 đến 5");
            }
            var entity = new DanhGiaSanPham
            {
                MaNguoiDung = newDanhGia.MaNguoiDung,
                MaSanPham = newDanhGia.MaSanPham,
                SoSao = newDanhGia.SoSao,
                BinhLuan = newDanhGia.BinhLuan,
                NgayDanhGia = DateTime.Now
            };

            qly.DanhGiaSanPhams.Add(entity);
            qly.SaveChanges();
            return Ok(entity);
        }
    }
}
