namespace DatSanPickleballBE.ModelDto
{
    public class ChiTietDonHangDto
    {
        public int MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
    }

    public class CreateDonHangDto
    {
        public int MaNguoiDung { get; set; }
        public int? MaTP { get; set; }
        public int? MaQH { get; set; }
        public string DiaChi { get; set; } = string.Empty;
        public decimal TongTien { get; set; }
        public List<ChiTietDonHangDto> ChiTietDonHangs { get; set; } = new();
    }
}
