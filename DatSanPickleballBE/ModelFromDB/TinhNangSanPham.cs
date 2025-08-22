using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("TinhNangSanPham")]
public partial class TinhNangSanPham
{
    [Key]
    [Column("maTinhNang")]
    public int MaTinhNang { get; set; }

    [Column("maSanPham")]
    public int? MaSanPham { get; set; }

    [Column("moTaTinhNang")]
    [StringLength(255)]
    public string? MoTaTinhNang { get; set; }

    [ForeignKey("MaSanPham")]
    [InverseProperty("TinhNangSanPhams")]
    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
