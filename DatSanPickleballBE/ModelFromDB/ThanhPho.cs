using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("ThanhPho")]
public partial class ThanhPho
{
    [Key]
    [Column("maTP")]
    public int MaTp { get; set; }

    [Column("tenTP")]
    [StringLength(100)]
    public string TenTp { get; set; } = null!;

    [InverseProperty("MaTpNavigation")]
    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    [InverseProperty("MaTpNavigation")]
    public virtual ICollection<QuanHuyen> QuanHuyens { get; set; } = new List<QuanHuyen>();
}
