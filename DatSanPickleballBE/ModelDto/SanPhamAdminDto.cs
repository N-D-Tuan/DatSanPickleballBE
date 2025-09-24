namespace DatSanPickleballBE.ModelDto
{
    public class SanPhamAdminDto
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int? MaDanhMuc { get; set; }
        public decimal? GiaNhap { get; set; }
        public decimal? GiaBan { get; set; }
        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }
        public int? SoLuongTon { get; set; }
        public List<string> Images { get; set; } = new();
        public List<string> Features { get; set; } = new();

    }
}
