using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("DanhMucSanPham")]
public partial class DanhMucSanPham
{
    [Key]
    [Column("maDanhMuc")]
    public int MaDanhMuc { get; set; }

    [Column("tenDanhMuc")]
    [StringLength(100)]
    public string TenDanhMuc { get; set; } = null!;

    [Column("moTa")]
    [StringLength(255)]
    public string? MoTa { get; set; }

    [InverseProperty("MaDanhMucNavigation")]
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
