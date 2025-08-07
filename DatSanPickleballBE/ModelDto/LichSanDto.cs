namespace DatSanPickleballBE.ModelDto
{
    public class LichSanDto
    {
        public int MaLichSan { get; set; }
        public int MaSan { get; set; }
        public int MaKhungGio { get; set; }
        public DateOnly Ngay { get; set; }
        public bool IsBooked { get; set; }
    }
}
