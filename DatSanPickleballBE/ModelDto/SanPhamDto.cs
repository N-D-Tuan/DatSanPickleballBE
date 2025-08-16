namespace DatSanPickleballBE.ModelDto
{
    public class SanPhamDto
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = null!;
        public int? SoLuongTon { get; set; }
        public decimal? GiaNhap { get; set; }
        public decimal? GiaBan { get; set; }
        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }
        public int? MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; } = null!;
    }
}
