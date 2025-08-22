using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("SanPham")]
public partial class SanPham
{
    [Key]
    [Column("maSanPham")]
    public int MaSanPham { get; set; }

    [Column("tenSanPham")]
    [StringLength(100)]
    public string TenSanPham { get; set; } = null!;

    [Column("soLuongTon")]
    public int? SoLuongTon { get; set; }

    [Column("giaNhap", TypeName = "decimal(18, 2)")]
    public decimal? GiaNhap { get; set; }

    [Column("giaBan", TypeName = "decimal(18, 2)")]
    public decimal? GiaBan { get; set; }

    [Column("hinhAnh")]
    [StringLength(255)]
    public string? HinhAnh { get; set; }

    [Column("moTa")]
    [StringLength(255)]
    public string? MoTa { get; set; }

    [Column("maDanhMuc")]
    public int? MaDanhMuc { get; set; }

    [InverseProperty("MaSanPhamNavigation")]
    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    [InverseProperty("MaSanPhamNavigation")]
    public virtual ICollection<DanhGiaSanPham> DanhGiaSanPhams { get; set; } = new List<DanhGiaSanPham>();

    [InverseProperty("MaSanPhamNavigation")]
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    [InverseProperty("MaSanPhamNavigation")]
    public virtual ICollection<HinhAnhSanPham> HinhAnhSanPhams { get; set; } = new List<HinhAnhSanPham>();

    [ForeignKey("MaDanhMuc")]
    [InverseProperty("SanPhams")]
    public virtual DanhMucSanPham? MaDanhMucNavigation { get; set; }

    [InverseProperty("MaSanPhamNavigation")]
    public virtual ICollection<TinhNangSanPham> TinhNangSanPhams { get; set; } = new List<TinhNangSanPham>();

    [ForeignKey("MaSanPham")]
    [InverseProperty("MaSanPhams")]
    public virtual ICollection<GiamGium> MaGiamGia { get; set; } = new List<GiamGium>();
}
