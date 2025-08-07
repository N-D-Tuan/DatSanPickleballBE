using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

[Table("Booking")]
public partial class Booking
{
    [Key]
    [Column("maBooking")]
    public int MaBooking { get; set; }

    [Column("maNguoiDung")]
    public int? MaNguoiDung { get; set; }

    [Column("maLichSan")]
    public int? MaLichSan { get; set; }

    [Column("trangThai")]
    [StringLength(50)]
    public string? TrangThai { get; set; }

    [ForeignKey("MaLichSan")]
    [InverseProperty("Bookings")]
    public virtual LichSan? MaLichSanNavigation { get; set; }

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("Bookings")]
    public virtual User? MaNguoiDungNavigation { get; set; }
}
