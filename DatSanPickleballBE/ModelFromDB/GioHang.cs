using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[PrimaryKey("MaNguoiDung", "MaSanPham")]
[Table("GioHang")]
public partial class GioHang
{
    [Key]
    [Column("maNguoiDung")]
    public int MaNguoiDung { get; set; }

    [Key]
    [Column("maSanPham")]
    public int MaSanPham { get; set; }

    [Column("soLuong")]
    public int? SoLuong { get; set; }

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("GioHangs")]
    public virtual User MaNguoiDungNavigation { get; set; } = null!;

    [ForeignKey("MaSanPham")]
    [InverseProperty("GioHangs")]
    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
