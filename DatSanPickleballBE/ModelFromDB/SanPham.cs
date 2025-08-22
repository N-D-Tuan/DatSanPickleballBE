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
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    [ForeignKey("MaDanhMuc")]
    [InverseProperty("SanPhams")]
    public virtual DanhMucSanPham? MaDanhMucNavigation { get; set; }

    [ForeignKey("MaSanPham")]
    [InverseProperty("MaSanPhams")]
    public virtual ICollection<GiamGia> MaGiamGia { get; set; } = new List<GiamGia>();
    public virtual ICollection<DoYeuThich> DoYeuThiches { get; set; } = new List<DoYeuThich>();

}
