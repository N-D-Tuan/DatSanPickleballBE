using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatSanPickleballBE.ModelFromDB
{
    [Table("DoYeuThich")]
    public class DoYeuThich
    {
        [Key, Column(Order = 0)]
        public int MaNguoiDung { get; set; }

        [Key, Column(Order = 1)]
        public int MaSanPham { get; set; }

        // Navigation
        public virtual User User { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}