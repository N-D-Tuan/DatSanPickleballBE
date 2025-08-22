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
    public virtual DbSet<DoYeuThich> DoYeuThiches { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DanhMucSanPham> DanhMucSanPhams { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GiamGia> GiamGia { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<KhungGio> KhungGios { get; set; }

    public virtual DbSet<LichSan> LichSans { get; set; }

    public virtual DbSet<San> Sans { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=TUAN\\MSSQLSERVER01;Initial Catalog=QuanLyDatSanPickleBall;Persist Security Info=True;User ID=sa;Password=123;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.MaBooking).HasName("PK__Booking__ED3802C25949C054");

            entity.ToTable("Booking", tb => tb.HasTrigger("trg_UpdateIsBooked"));

            entity.HasOne(d => d.MaLichSanNavigation).WithMany(p => p.Bookings).HasConstraintName("FK__Booking__maLichS__44FF419A");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Bookings).HasConstraintName("FK__Booking__maNguoi__440B1D61");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.MaDonHang, e.MaSanPham }).HasName("PK__ChiTietD__A2A901DD18D7E5A9");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDonHangs).HasConstraintName("FK__ChiTietDo__maDon__02084FDA");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietDonHangs).HasConstraintName("FK__ChiTietDo__maSan__02FC7413");
        });

        modelBuilder.Entity<DanhMucSanPham>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DanhMucS__6B0F914CF52E6994");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonHang__871D38198CD80D2F");

            entity.Property(e => e.NgayDat).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DonHang__maNguoi__7D439ABD");
        });

        modelBuilder.Entity<GiamGia>(entity =>
        {
            entity.HasKey(e => e.MaGiamGia).HasName("PK__GiamGia__F26B142D54329B1E");

            entity.Property(e => e.SoLanDaSuDung).HasDefaultValue(0);
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => new { e.MaNguoiDung, e.MaSanPham }).HasName("PK__GioHang__61D0002EC77783CD");

            entity.Property(e => e.SoLuong).HasDefaultValue(1);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.GioHangs).HasConstraintName("FK__GioHang__maNguoi__76969D2E");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.GioHangs).HasConstraintName("FK__GioHang__maSanPh__778AC167");
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

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__5B439C433E5D5B39");

            entity.Property(e => e.SoLuongTon).HasDefaultValue(0);

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPhams)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SanPham__maDanhM__656C112C");

            entity.HasMany(d => d.MaGiamGia).WithMany(p => p.MaSanPhams)
                .UsingEntity<Dictionary<string, object>>(
                    "SanPhamGiamGium",
                    r => r.HasOne<GiamGia>().WithMany()
                        .HasForeignKey("MaGiamGia")
                        .HasConstraintName("FK__SanPhamGi__maGia__71D1E811"),
                    l => l.HasOne<SanPham>().WithMany()
                        .HasForeignKey("MaSanPham")
                        .HasConstraintName("FK__SanPhamGi__maSan__70DDC3D8"),
                    j =>
                    {
                        j.HasKey("MaSanPham", "MaGiamGia").HasName("PK__SanPhamG__94652D010C86C6AC");
                        j.ToTable("SanPhamGiamGia");
                        j.IndexerProperty<int>("MaSanPham").HasColumnName("maSanPham");
                        j.IndexerProperty<int>("MaGiamGia").HasColumnName("maGiamGia");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__User__446439EA5A7294C1");

            entity.HasIndex(e => e.ResetOtp, "UQ_User_ResetOtp_NotNull")
                .IsUnique()
                .HasFilter("([ResetOtp] IS NOT NULL)");
        });

        modelBuilder.Entity<DoYeuThich>(entity =>
        {
            entity.HasKey(e => new { e.MaNguoiDung, e.MaSanPham });

            entity.HasOne(e => e.User)
                  .WithMany(u => u.DoYeuThiches)
                  .HasForeignKey(e => e.MaNguoiDung)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.SanPham)
                  .WithMany(s => s.DoYeuThiches)
                  .HasForeignKey(e => e.MaSanPham)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
