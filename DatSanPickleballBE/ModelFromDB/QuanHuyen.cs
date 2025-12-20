using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("QuanHuyen")]
public partial class QuanHuyen
{
    [Key]
    [Column("maQH")]
    public int MaQh { get; set; }

    [Column("tenQH")]
    [StringLength(100)]
    public string TenQh { get; set; } = null!;

    [Column("maTP")]
    public int MaTp { get; set; }

    [ForeignKey("MaTp")]
    [InverseProperty("QuanHuyens")]
    public virtual ThanhPho MaTpNavigation { get; set; } = null!;
}
