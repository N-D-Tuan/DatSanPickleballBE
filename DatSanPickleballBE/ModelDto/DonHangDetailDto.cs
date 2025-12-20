namespace DatSanPickleballBE.ModelDto
{
    public class DonHangDetailDto
    {
        public int MaDonHang { get; set; }
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = null!;
        public string? HinhAnh { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }
}
