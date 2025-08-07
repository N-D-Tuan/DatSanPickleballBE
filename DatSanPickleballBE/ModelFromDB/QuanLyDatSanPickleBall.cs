using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.ModelFromDB;

public partial class QuanLyDatSanPickleBall : DbContext
{
    public QuanLyDatSanPickleBall()
    {
    }

    public QuanLyDatSanPickleBall(DbContextOptions<QuanLyDatSanPickleBall> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<KhungGio> KhungGios { get; set; }

    public virtual DbSet<LichSan> LichSans { get; set; }

    public virtual DbSet<San> Sans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=TUAN\\MSSQLSERVER01;Initial Catalog=QuanLyDatSanPickleBall;Persist Security Info=True;User ID=sa;Password=123;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.MaBooking).HasName("PK__Booking__ED3802C25949C054");

            entity.HasOne(d => d.MaLichSanNavigation).WithMany(p => p.Bookings).HasConstraintName("FK__Booking__maLichS__44FF419A");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Bookings).HasConstraintName("FK__Booking__maNguoi__440B1D61");
        });

        modelBuilder.Entity<KhungGio>(entity =>
        {
            entity.HasKey(e => e.MaKhungGio).HasName("PK__KhungGio__D0AD3B33D9DA1198");

            entity.Property(e => e.MaKhungGio).ValueGeneratedNever();
        });

        modelBuilder.Entity<LichSan>(entity =>
        {
            entity.HasKey(e => e.MaLichSan).HasName("PK__LichSan__FFB5B701E368607C");

            entity.Property(e => e.IsBooked).HasDefaultValue(false);

            entity.HasOne(d => d.MaKhungGioNavigation).WithMany(p => p.LichSans).HasConstraintName("FK__LichSan__maKhung__403A8C7D");

            entity.HasOne(d => d.MaSanNavigation).WithMany(p => p.LichSans).HasConstraintName("FK__LichSan__maSan__3F466844");
        });

        modelBuilder.Entity<San>(entity =>
        {
            entity.HasKey(e => e.MaSan).HasName("PK__San__0C895661CEFB3BC4");

            entity.Property(e => e.MaSan).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__User__446439EA5A7294C1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
