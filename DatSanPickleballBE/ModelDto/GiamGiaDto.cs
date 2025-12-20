using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatSanPickleballBE.ModelDto
{
    public class GiamGiaDto
    {
        public int MaGiamGia { get; set; }

        public string? MaCode { get; set; }

        public string? MoTa { get; set; }

        public string? LoaiGiamGia { get; set; }

        public decimal? GiaTri { get; set; }

        public DateOnly? NgayBatDau { get; set; }

        public DateOnly? NgayKetThuc { get; set; }

        public int? SoLanSuDungMax { get; set; }

        public int? SoLanDaSuDung { get; set; }

        public string? TrangThai { get; set; }
    }
}
