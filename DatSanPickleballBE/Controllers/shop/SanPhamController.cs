using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatSanPickleballBE.Controllers.shop
{
    [Route("[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public SanPhamController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }
        [HttpGet]
        [Route("/SanPham/ListAll")]
        public IActionResult GetAll()
        {
            var result = from sp in qly.SanPhams
                         join dm in qly.DanhMucSanPhams on sp.MaDanhMuc equals dm.MaDanhMuc 
                         select new SanPhamDto
                        {
                            MaSanPham = sp.MaSanPham,
                            TenSanPham = sp.TenSanPham,
                            SoLuongTon = sp.SoLuongTon,
                            GiaNhap = sp.GiaNhap,
                            GiaBan = sp.GiaBan,  
                            HinhAnh = sp.HinhAnh,
                            MoTa = sp.MoTa,
                            MaDanhMuc = sp.MaDanhMuc,
                            TenDanhMuc = dm.TenDanhMuc
                        }; 
            return Ok(result);
        }
        [HttpGet]
        [Route("/SanPham/List/{maDanhMuc}")]
        public IActionResult GetSanPhamTheoMaDanhMuc(int maDanhMuc)
        {
            var result = qly.SanPhams.Where(s => s.MaDanhMuc == maDanhMuc).Select(s => new SanPhamDto
            {
                MaSanPham = s.MaSanPham,
                TenSanPham = s.TenSanPham,
                SoLuongTon = s.SoLuongTon,
                GiaNhap = s.GiaNhap,
                GiaBan = s.GiaBan,
                HinhAnh = s.HinhAnh,
                MoTa = s.MoTa,
                MaDanhMuc = s.MaDanhMuc,
            });
            return Ok(result);
        }
        [HttpPost]
        [Route("/SanPham/Insert")]
        public IActionResult Insert([FromBody] SanPhamDto newSanPham)
        {
            if(newSanPham == null)
            {
                return BadRequest("Dữ liệu sản phẩm không hợp lệ");
            }
            var entity = new SanPham
            {
                MaSanPham = newSanPham.MaSanPham,
                TenSanPham = newSanPham.TenSanPham,
                SoLuongTon = newSanPham.SoLuongTon,
                GiaNhap = newSanPham.GiaNhap,
                GiaBan = newSanPham.GiaBan,
                HinhAnh = newSanPham.HinhAnh,
                MoTa = newSanPham.MoTa,
                MaDanhMuc = newSanPham.MaDanhMuc
            };

            qly.SanPhams.Add(entity);
            qly.SaveChanges();
            return Ok(entity);
        }
    }
}
