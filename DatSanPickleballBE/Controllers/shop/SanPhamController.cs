using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        //lấy cho trang chủ
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

        //lấy cho trang detail
        [HttpGet]
        [Route("/SanPham/ListAllForDetail")]
        public IActionResult GetAllForDetail()
        {
            var result = (from sp in qly.SanPhams
                         join dm in qly.DanhMucSanPhams on sp.MaDanhMuc equals dm.MaDanhMuc
                         select new SanPhamDetailDto
                         {
                             Id = sp.MaSanPham,
                             Name = sp.TenSanPham,
                             Category = dm.TenDanhMuc,
                             Price = sp.GiaBan,
                             OriginalPrice = sp.GiaBan, // nếu muốn hiện giá gốc
                             Image = sp.HinhAnh,
                             Description = sp.MoTa,
                             StockQuantity = sp.SoLuongTon,
                             Badge = "sale",
                         }).ToList();
            
            if (result == null)
                return NotFound();

            foreach(var sp in result)
            {
                sp.Images = qly.HinhAnhSanPhams
                    .Where(h => h.MaSanPham == sp.Id)
                    .Select(h => h.HinhAnh)
                    .ToList();

                sp.Features = qly.TinhNangSanPhams
                    .Where(t => t.MaSanPham == sp.Id)
                    .Select(t => t.MoTaTinhNang)
                    .ToList();

                var danhGia = qly.DanhGiaSanPhams
                    .Where(d => d.MaSanPham == sp.Id);

                sp.Rating = danhGia.Any() ? danhGia.Average(d => d.SoSao) : 0;
                sp.Reviews = danhGia.Count();
            }

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
        [HttpGet]
        [Route("/SanPham/MaSanPham/{maSP}")]
        public IActionResult GetSanPhamTheoMaSP(int maSP)
        {
            var result = qly.SanPhams.Where(sp => sp.MaSanPham == maSP).Select(s => new SanPhamDto
            {
                MaSanPham = s.MaSanPham,
                TenSanPham = s.TenSanPham,
                SoLuongTon = s.SoLuongTon,
                GiaNhap = s.GiaNhap,
                GiaBan = s.GiaBan,
                HinhAnh = s.HinhAnh,
                MoTa = s.MoTa,
                MaDanhMuc = s.MaDanhMuc,
            }).FirstOrDefault();
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
        // GET: api/DoYeuThich/{maNguoiDung}
        [HttpGet("DoYeuThich/{maNguoiDung}")]
        public async Task<IActionResult> GetDoYeuThichByNguoiDung(int maNguoiDung)
        {
            var items = await qly.DoYeuThiches
                .Where(d => d.MaNguoiDung == maNguoiDung)
                .Include(d => d.SanPham)
                .Select(d => new DoYeuThichDto
                {
                    MaNguoiDung = d.MaNguoiDung,
                    MaSanPham = d.MaSanPham,
                    TenSanPham = d.SanPham.TenSanPham,
                    GiaBan = d.SanPham.GiaBan,
                    HinhAnh = d.SanPham.HinhAnh,
                    MoTa = d.SanPham.MoTa,
                    SoLuongTon = d.SanPham.SoLuongTon,
                    MaDanhMuc = d.SanPham.MaDanhMuc ?? 0,
                    TenDanhMuc = d.SanPham.MaDanhMucNavigation.TenDanhMuc
                })
                .ToListAsync();

            if (!items.Any())
                return NotFound("Người dùng chưa có sản phẩm yêu thích nào.");

            return Ok(items);
        }

        // POST: api/DoYeuThich
        [HttpPost("create/DoYeuThich/")]
        public async Task<IActionResult> AddDoYeuThich([FromBody] CreateDoYeuThichDto model)
        {
            if (model == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            // Kiểm tra đã tồn tại chưa
            var exists = await qly.DoYeuThiches
                .AnyAsync(d => d.MaNguoiDung == model.MaNguoiDung && d.MaSanPham == model.MaSanPham);

            if (exists)
                return Conflict("Sản phẩm này đã có trong danh sách yêu thích.");

            // Dùng raw SQL để INSERT
            var sql = "INSERT INTO DoYeuThich (MaNguoiDung, MaSanPham) VALUES (@p0, @p1)";
            await qly.Database.ExecuteSqlRawAsync(sql, model.MaNguoiDung, model.MaSanPham);

            return Ok("Đã thêm vào danh sách yêu thích.");
        }

        // DELETE: api/DoYeuThich/{maNguoiDung}/{maSanPham}
        [HttpDelete("Delete/{maNguoiDung}/{maSanPham}")]
        public async Task<IActionResult> DeleteDoYeuThich(int maNguoiDung, int maSanPham)
        {
            var item = await qly.DoYeuThiches
                .FirstOrDefaultAsync(d => d.MaNguoiDung == maNguoiDung && d.MaSanPham == maSanPham);

            if (item == null)
                return NotFound("Không tìm thấy sản phẩm trong danh sách yêu thích.");

            qly.DoYeuThiches.Remove(item);
            await qly.SaveChangesAsync();

            return Ok("Đã xóa sản phẩm khỏi danh sách yêu thích.");
        }
    }
}
