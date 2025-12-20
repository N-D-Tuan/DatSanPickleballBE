namespace DatSanPickleballBE.ModelDto
{
    public class DanhGiaSanPhamDto
    {
        public int MaDanhGia { get; set; }
        public int? MaNguoiDung { get; set; }
        public int? MaSanPham { get; set; }
        public int? SoSao { get; set; }
        public string? BinhLuan { get; set; }
        public DateTime? NgayDanhGia { get; set; }
    }
}
