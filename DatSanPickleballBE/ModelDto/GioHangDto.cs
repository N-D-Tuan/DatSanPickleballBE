namespace DatSanPickleballBE.ModelDto
{
    public class GioHangDto
    {
        public int MaNguoiDung { get; set; }
        public int MaSanPham { get; set; }
        public int? SoLuong { get; set; }
        public string TenSanPham { get; set; } = null!;
        public decimal? GiaBan { get; set; }
        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }
        public int? MaDanhMuc { get; set; }
        public int? SoLuongTon { get; set; }
        public string TenDanhMuc { get; set; } = null!;
    }
}
