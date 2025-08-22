using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("DanhGiaSanPham")]
public partial class DanhGiaSanPham
{
    [Key]
    [Column("maDanhGia")]
    public int MaDanhGia { get; set; }

    [Column("maNguoiDung")]
    public int? MaNguoiDung { get; set; }

    [Column("maSanPham")]
    public int? MaSanPham { get; set; }

    [Column("soSao")]
    public int? SoSao { get; set; }

    [Column("binhLuan")]
    public string? BinhLuan { get; set; }

    [Column("ngayDanhGia", TypeName = "datetime")]
    public DateTime? NgayDanhGia { get; set; }

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("DanhGiaSanPhams")]
    public virtual User? MaNguoiDungNavigation { get; set; }

    [ForeignKey("MaSanPham")]
    [InverseProperty("DanhGiaSanPhams")]
    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
