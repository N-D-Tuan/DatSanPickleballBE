namespace DatSanPickleballBE.ModelDto
{
    public class DonHangDto
    {
        public int MaDonHang { get; set; }
        public int MaNguoiDung { get; set; }
        public string TenNguoiDung { get; set; } = null!;
        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = null!;

        public int MaTP { get; set; }
        public string TenTP { get; set; } = null!;

        public string DiaChi { get; set; } = null!;

        public int MaQH { get; set; }
        public string TenQH { get; set; } = null!;
        public List<DonHangDetailDto> DonHangDetails { get; set; } = new List<DonHangDetailDto>();
    }
}
