namespace DatSanPickleballBE.ModelDto
{
    public class HistoryBookingDto
    {
        public int MaBooking { get; set; }
        public int MaNguoiDung { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public int MaLichSan { get; set; }
        public string TenSan { get; set; } = string.Empty;
        public DateOnly? Ngay { get; set; }
        public TimeOnly? GioBatDau { get; set; }
        public TimeOnly? GioKetThuc { get; set; }
    }
}
