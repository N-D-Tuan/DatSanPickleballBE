using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Index("MaCode", Name = "UQ__GiamGia__366294EBCA013844", IsUnique = true)]
public partial class GiamGium
{
    [Key]
    [Column("maGiamGia")]
    public int MaGiamGia { get; set; }

    [Column("maCode")]
    [StringLength(50)]
    public string? MaCode { get; set; }

    [Column("moTa")]
    [StringLength(255)]
    public string? MoTa { get; set; }

    [Column("loaiGiamGia")]
    [StringLength(20)]
    public string? LoaiGiamGia { get; set; }

    [Column("giaTri", TypeName = "decimal(18, 2)")]
    public decimal? GiaTri { get; set; }

    [Column("ngayBatDau")]
    public DateOnly? NgayBatDau { get; set; }

    [Column("ngayKetThuc")]
    public DateOnly? NgayKetThuc { get; set; }

    [Column("soLanSuDungMax")]
    public int? SoLanSuDungMax { get; set; }

    [Column("soLanDaSuDung")]
    public int? SoLanDaSuDung { get; set; }

    [Column("trangThai")]
    [StringLength(20)]
    public string? TrangThai { get; set; }

    [ForeignKey("MaGiamGia")]
    [InverseProperty("MaGiamGia")]
    public virtual ICollection<SanPham> MaSanPhams { get; set; } = new List<SanPham>();
}
