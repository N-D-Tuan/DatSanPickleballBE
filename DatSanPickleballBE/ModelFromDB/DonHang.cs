using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("DonHang")]
public partial class DonHang
{
    [Key]
    [Column("maDonHang")]
    public int MaDonHang { get; set; }

    [Column("maNguoiDung")]
    public int? MaNguoiDung { get; set; }

    [Column("ngayDat", TypeName = "datetime")]
    public DateTime? NgayDat { get; set; }

    [Column("trangThai")]
    [StringLength(50)]
    public string? TrangThai { get; set; }

    [Column("tongTien", TypeName = "decimal(18, 2)")]
    public decimal? TongTien { get; set; }

    [Column("maTP")]
    public int? MaTP { get; set; }

    [Column("diaChi")]
    public string? DiaChi { get; set; }

    [Column("maQH")]
    public int? MaQH { get; set; }

    [InverseProperty("MaDonHangNavigation")]
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("DonHangs")]
    public virtual User? MaNguoiDungNavigation { get; set; }

    [ForeignKey("MaTP")]
    [InverseProperty("DonHangs")]
    public virtual ThanhPho? MaTpNavigation { get; set; }
}
