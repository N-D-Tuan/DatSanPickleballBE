using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("HinhAnhSanPham")]
public partial class HinhAnhSanPham
{
    [Key]
    [Column("maHinhAnh")]
    public int MaHinhAnh { get; set; }

    [Column("maSanPham")]
    public int? MaSanPham { get; set; }

    [Column("hinhAnh")]
    [StringLength(255)]
    public string? HinhAnh { get; set; }

    [ForeignKey("MaSanPham")]
    [InverseProperty("HinhAnhSanPhams")]
    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
