using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[PrimaryKey("MaDonHang", "MaSanPham")]
[Table("ChiTietDonHang")]
public partial class ChiTietDonHang
{
    [Key]
    [Column("maDonHang")]
    public int MaDonHang { get; set; }

    [Key]
    [Column("maSanPham")]
    public int MaSanPham { get; set; }

    [Column("soLuong")]
    public int? SoLuong { get; set; }

    [Column("donGia", TypeName = "decimal(18, 2)")]
    public decimal? DonGia { get; set; }

    [ForeignKey("MaDonHang")]
    [InverseProperty("ChiTietDonHangs")]
    public virtual DonHang MaDonHangNavigation { get; set; } = null!;

    [ForeignKey("MaSanPham")]
    [InverseProperty("ChiTietDonHangs")]
    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
