namespace DatSanPickleballBE.ModelDto
{
    public class ApDungGiamGiaRequest
    {
        public int MaGiamGia { get; set; }
        public List<int> DanhSachSanPham { get; set; } = new();
    }
}
