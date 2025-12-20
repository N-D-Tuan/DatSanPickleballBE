using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("User")]
public partial class User
{
    [Key]
    [Column("maNguoiDung")]
    public int MaNguoiDung { get; set; }

    [Column("tenNguoiDung")]
    [StringLength(100)]
    public string? TenNguoiDung { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("soDienThoai")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Column("matKhau")]
    [StringLength(100)]
    public string? MatKhau { get; set; }

    [Column("role")]
    [StringLength(50)]
    public string? Role { get; set; }

    [Column("ResetOtp ")]
    [StringLength(200)]
    public string? ResetOtp { get; set; }

    [Column("ResetOtpExpiry ", TypeName = "datetime")]
    public DateTime? ResetOtpExpiry { get; set; }

    [InverseProperty("MaNguoiDungNavigation")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("MaNguoiDungNavigation")]
    public virtual ICollection<DanhGiaSanPham> DanhGiaSanPhams { get; set; } = new List<DanhGiaSanPham>();

    [InverseProperty("MaNguoiDungNavigation")]
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    [InverseProperty("MaNguoiDungNavigation")]
    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
    public virtual ICollection<DoYeuThich> DoYeuThiches { get; set; } = new List<DoYeuThich>();

}
