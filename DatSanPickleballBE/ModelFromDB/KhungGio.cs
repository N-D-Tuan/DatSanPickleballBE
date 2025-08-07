using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("KhungGio")]
public partial class KhungGio
{
    [Key]
    [Column("maKhungGio")]
    public int MaKhungGio { get; set; }

    [Column("gioBatDau")]
    public TimeOnly? GioBatDau { get; set; }

    [Column("gioKetThuc")]
    public TimeOnly? GioKetThuc { get; set; }

    [InverseProperty("MaKhungGioNavigation")]
    public virtual ICollection<LichSan> LichSans { get; set; } = new List<LichSan>();
}
