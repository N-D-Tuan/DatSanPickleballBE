using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("San")]
public partial class San
{
    [Key]
    [Column("maSan")]
    public int MaSan { get; set; }

    [Column("tenSan")]
    [StringLength(100)]
    public string? TenSan { get; set; }

    [Column("kieuSan")]
    [StringLength(50)]
    public string? KieuSan { get; set; }

    [Column("trangThai")]
    [StringLength(50)]
    public string? TrangThai { get; set; }

    [Column("viTri")]
    [StringLength(100)]
    public string? ViTri { get; set; }

    [Column("hinhAnh")]
    [Unicode(false)]
    public string? HinhAnh { get; set; }

    [Column("gia")]
    public int? Gia { get; set; }

    [InverseProperty("MaSanNavigation")]
    public virtual ICollection<LichSan> LichSans { get; set; } = new List<LichSan>();
}
