using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("LichSan")]
public partial class LichSan
{
    [Key]
    [Column("maLichSan")]
    public int MaLichSan { get; set; }

    [Column("maSan")]
    public int? MaSan { get; set; }

    [Column("maKhungGio")]
    public int? MaKhungGio { get; set; }

    [Column("ngay")]
    public DateOnly? Ngay { get; set; }

    [Column("isBooked")]
    public bool? IsBooked { get; set; }

    [InverseProperty("MaLichSanNavigation")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [ForeignKey("MaKhungGio")]
    [InverseProperty("LichSans")]
    public virtual KhungGio? MaKhungGioNavigation { get; set; }

    [ForeignKey("MaSan")]
    [InverseProperty("LichSans")]
    public virtual San? MaSanNavigation { get; set; }
}
