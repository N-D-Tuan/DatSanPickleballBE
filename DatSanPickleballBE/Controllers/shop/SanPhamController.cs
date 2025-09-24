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
            var result = qly.SanPhams
                        .Include(sp => sp.MaDanhMucNavigation)
                        .Include(sp => sp.MaGiamGia)
                        .AsEnumerable()
                        .Select(sp => 
                        {
                            var giamGiaSoTien = sp.MaGiamGia
                                .Where(gg => gg.LoaiGiamGia == "sotien" && gg.TrangThai == "HoatDong")
                                .Select(gg => gg.GiaTri)
                                .DefaultIfEmpty(0)
                                .Max();

                            var giamGiaPhanTram = sp.MaGiamGia
                                .Where(gg => gg.LoaiGiamGia == "phantram" && gg.TrangThai == "HoatDong")
                                .Select(gg => gg.GiaTri)
                                .DefaultIfEmpty(0)
                                .Max();

                            var giaBanSauTien = sp.GiaBan
                                                - giamGiaSoTien
                                                - (sp.GiaBan * giamGiaPhanTram / 100);

                            if (giaBanSauTien < 0) giaBanSauTien = 0;

                            return new SanPhamAllDto
                            {
                                MaSanPham = sp.MaSanPham,
                                TenSanPham = sp.TenSanPham,
                                SoLuongTon = sp.SoLuongTon,
                                GiaNhap = sp.GiaNhap,
                                GiaBan = sp.GiaBan,
                                HinhAnh = sp.HinhAnh,
                                MoTa = sp.MoTa,
                                MaDanhMuc = sp.MaDanhMuc,
                                TenDanhMuc = sp.MaDanhMucNavigation.TenDanhMuc,
                                OriginalPrice = giaBanSauTien
                            };
                            
                        }); 
            return Ok(result);
        }

        //lấy cho trang detail
        [HttpGet]
        [Route("/SanPham/ListAllForDetail")]
        public IActionResult GetAllForDetail()
        {
            var result = qly.SanPhams
                .Include(sp => sp.MaDanhMucNavigation)
                .Include(sp => sp.MaGiamGia)
                .AsEnumerable()
                .Select(sp => 
                {
                    var giamGiaSoTien = sp.MaGiamGia
                                .Where(gg => gg.LoaiGiamGia == "sotien" && gg.TrangThai == "HoatDong")
                                .Select(gg => gg.GiaTri)
                                .DefaultIfEmpty(0)
                                .Max();

                    var giamGiaPhanTram = sp.MaGiamGia
                                .Where(gg => gg.LoaiGiamGia == "phantram" && gg.TrangThai == "HoatDong")
                                .Select(gg => gg.GiaTri)
                                .DefaultIfEmpty(0)
                                .Max();

                    var giaBanSauTien = sp.GiaBan
                                        - giamGiaSoTien
                                        - (sp.GiaBan * giamGiaPhanTram / 100);

                    if (giaBanSauTien < 0) giaBanSauTien = 0;

                    return new SanPhamDetailDto
                    {
                        Id = sp.MaSanPham,
                        Name = sp.TenSanPham,
                        Category = sp.MaDanhMucNavigation.TenDanhMuc,
                        Price = giaBanSauTien,
                        OriginalPrice = sp.GiaBan, // nếu muốn hiện giá gốc
                        Image = sp.HinhAnh,
                        Description = sp.MoTa,
                        StockQuantity = sp.SoLuongTon,
                        Badge = "sale",
                    };  
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
        [Route("/SanPham/Detail/{maSanPham}")]
        public IActionResult GetDetail(int maSanPham)
        {
            var sp = qly.SanPhams
                .Include(s => s.MaDanhMucNavigation)
                .Include(s => s.MaGiamGia)
                .FirstOrDefault(s => s.MaSanPham == maSanPham);

            if (sp == null)
                return NotFound();

            var result = new SanPhamAdminDto
            {
                MaSanPham = sp.MaSanPham,
                TenSanPham = sp.TenSanPham,
                MaDanhMuc = sp.MaDanhMuc,
                GiaNhap = sp.GiaNhap,
                GiaBan = sp.GiaBan,
                HinhAnh = sp.HinhAnh,
                MoTa = sp.MoTa,
                SoLuongTon = sp.SoLuongTon,
            };

            // Load thêm ảnh
            result.Images = qly.HinhAnhSanPhams
                .Where(h => h.MaSanPham == sp.MaSanPham)
                .Select(h => h.HinhAnh)
                .ToList();

            // Load thêm tính năng
            result.Features = qly.TinhNangSanPhams
                .Where(t => t.MaSanPham == sp.MaSanPham)
                .Select(t => t.MoTaTinhNang)
                .ToList();

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
        public IActionResult Insert([FromBody] SanPhamAdminDto newSanPham)
        {
            if (newSanPham == null)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            // Khởi tạo sản phẩm mới
            var sp = new SanPham
            {
                TenSanPham = newSanPham.TenSanPham,
                MaDanhMuc = newSanPham.MaDanhMuc,
                GiaNhap = newSanPham.GiaNhap ?? 0,
                GiaBan = newSanPham.GiaBan ?? 0,
                MoTa = newSanPham.MoTa,
                HinhAnh = newSanPham.HinhAnh,
                SoLuongTon = newSanPham.SoLuongTon ?? 0
            };

            qly.SanPhams.Add(sp);
            try
            {
                qly.SaveChanges(); // lưu trước để có MaSanPham (identity từ DB)
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi thêm sản phẩm: " + ex.Message);
            }

            // =============================
            // Xử lý Images
            // =============================
            if (newSanPham.Images != null && newSanPham.Images.Any())
            {
                foreach (var img in newSanPham.Images)
                {
                    qly.HinhAnhSanPhams.Add(new HinhAnhSanPham
                    {
                        MaSanPham = sp.MaSanPham,
                        HinhAnh = img
                    });
                }
            }

            // =============================
            // Xử lý Features
            // =============================
            if (newSanPham.Features != null && newSanPham.Features.Any())
            {
                foreach (var ft in newSanPham.Features)
                {
                    qly.TinhNangSanPhams.Add(new TinhNangSanPham
                    {
                        MaSanPham = sp.MaSanPham,
                        MoTaTinhNang = ft
                    });
                }
            }

            try
            {
                qly.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi thêm ảnh/tính năng: " + ex.Message);
            }

            return Ok(new { message = "Thêm sản phẩm thành công", sp.MaSanPham });
        }
        [HttpPut]
        [Route("/SanPham/Update/{maSanPham}")]
        public IActionResult Update(int maSanPham, [FromBody] SanPhamAdminDto dto)
        {
            if (dto == null || maSanPham != dto.MaSanPham)
            {
                return BadRequest("Dữ liệu không hợp lệ");
            }

            var sp = qly.SanPhams.FirstOrDefault(s => s.MaSanPham == maSanPham);
            if (sp == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            // Cập nhật thông tin cơ bản
            sp.TenSanPham = dto.TenSanPham;
            sp.MaDanhMuc = dto.MaDanhMuc;
            sp.GiaNhap = dto.GiaNhap ?? sp.GiaNhap;  // giá nhập
            sp.GiaBan = dto.GiaBan ?? sp.GiaBan;  // giá gốc
            sp.MoTa = dto.MoTa;
            sp.HinhAnh = dto.HinhAnh;
            sp.SoLuongTon = dto.SoLuongTon ?? sp.SoLuongTon;

            // =============================
            // Xử lý Images (xóa và thêm mới)
            // =============================
            var oldImages = qly.HinhAnhSanPhams.Where(h => h.MaSanPham == sp.MaSanPham).ToList();
            if (oldImages.Any())
            {
                qly.HinhAnhSanPhams.RemoveRange(oldImages);
            }

            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var img in dto.Images)
                {
                    qly.HinhAnhSanPhams.Add(new HinhAnhSanPham
                    {
                        MaSanPham = sp.MaSanPham,
                        HinhAnh = img
                    });
                }
            }

            // =============================
            // Xử lý Features (xóa và thêm mới)
            // =============================
            var oldFeatures = qly.TinhNangSanPhams.Where(t => t.MaSanPham == sp.MaSanPham).ToList();
            if (oldFeatures.Any())
            {
                qly.TinhNangSanPhams.RemoveRange(oldFeatures);
            }

            if (dto.Features != null && dto.Features.Any())
            {
                foreach (var ft in dto.Features)
                {
                    qly.TinhNangSanPhams.Add(new TinhNangSanPham
                    {
                        MaSanPham = sp.MaSanPham,
                        MoTaTinhNang = ft
                    });
                }
            }

            try
            {
                qly.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }

            return Ok(new { message = "Cập nhật sản phẩm thành công", dto.MaSanPham });
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

        [HttpDelete]
        [Route("/SanPham/Delete/{maSanPham}")]
        public IActionResult Delete(int maSanPham)
        {
            var sp = qly.SanPhams.FirstOrDefault(s => s.MaSanPham == maSanPham);
            if (sp == null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            try
            {
                // Xóa images liên quan
                var images = qly.HinhAnhSanPhams.Where(h => h.MaSanPham == maSanPham).ToList();
                if (images.Any())
                {
                    qly.HinhAnhSanPhams.RemoveRange(images);
                }

                // Xóa features liên quan
                var features = qly.TinhNangSanPhams.Where(t => t.MaSanPham == maSanPham).ToList();
                if (features.Any())
                {
                    qly.TinhNangSanPhams.RemoveRange(features);
                }

                // Xóa sản phẩm
                qly.SanPhams.Remove(sp);

                qly.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi khi xóa sản phẩm: " + ex.Message);
            }

            return Ok(new { message = "Xóa sản phẩm thành công", maSanPham });
        }

    }
}
