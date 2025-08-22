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

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<DanhGiaSanPham> DanhGiaSanPhams { get; set; }

    public virtual DbSet<DanhMucSanPham> DanhMucSanPhams { get; set; }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<GiamGium> GiamGia { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HinhAnhSanPham> HinhAnhSanPhams { get; set; }

    public virtual DbSet<KhungGio> KhungGios { get; set; }

    public virtual DbSet<LichSan> LichSans { get; set; }

    public virtual DbSet<San> Sans { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TinhNangSanPham> TinhNangSanPhams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PHAT;Initial Catalog=QuanLyDatSanPickleBall;Persist Security Info=True;User ID=sa;Password=123;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.MaBooking).HasName("PK__Booking__ED3802C2E49A85A0");

            entity.ToTable("Booking", tb => tb.HasTrigger("trg_UpdateIsBooked"));

            entity.HasOne(d => d.MaLichSanNavigation).WithMany(p => p.Bookings).HasConstraintName("FK__Booking__maLichS__44FF419A");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Bookings).HasConstraintName("FK__Booking__maNguoi__440B1D61");
        });

        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.MaDonHang, e.MaSanPham }).HasName("PK__ChiTietD__A2A901DD7264A359");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.ChiTietDonHangs).HasConstraintName("FK__ChiTietDo__maDon__00200768");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietDonHangs).HasConstraintName("FK__ChiTietDo__maSan__01142BA1");
        });

        modelBuilder.Entity<DanhGiaSanPham>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DanhGiaS__6B15DD9A25EC03D7");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DanhGiaSanPhams)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DanhGiaSa__maNgu__04E4BC85");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.DanhGiaSanPhams)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DanhGiaSa__maSan__05D8E0BE");
        });

        modelBuilder.Entity<DanhMucSanPham>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DanhMucS__6B0F914C263D9256");
        });

        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DonHang__871D381933E7876F");

            entity.Property(e => e.NgayDat).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.DonHangs)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DonHang__maNguoi__7B5B524B");
        });

        modelBuilder.Entity<GiamGium>(entity =>
        {
            entity.HasKey(e => e.MaGiamGia).HasName("PK__GiamGia__F26B142D96DD9B3B");

            entity.Property(e => e.SoLanDaSuDung).HasDefaultValue(0);
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => new { e.MaNguoiDung, e.MaSanPham }).HasName("PK__GioHang__61D0002EA992243D");

            entity.Property(e => e.SoLuong).HasDefaultValue(1);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.GioHangs).HasConstraintName("FK__GioHang__maNguoi__74AE54BC");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.GioHangs).HasConstraintName("FK__GioHang__maSanPh__75A278F5");
        });

        modelBuilder.Entity<HinhAnhSanPham>(entity =>
        {
            entity.HasKey(e => e.MaHinhAnh).HasName("PK__HinhAnhS__134CD06C64E39562");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.HinhAnhSanPhams)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__HinhAnhSa__maSan__08B54D69");
        });

        modelBuilder.Entity<KhungGio>(entity =>
        {
            entity.HasKey(e => e.MaKhungGio).HasName("PK__KhungGio__D0AD3B33D6C68606");

            entity.Property(e => e.MaKhungGio).ValueGeneratedNever();
        });

        modelBuilder.Entity<LichSan>(entity =>
        {
            entity.HasKey(e => e.MaLichSan).HasName("PK__LichSan__FFB5B70126154B67");

            entity.Property(e => e.IsBooked).HasDefaultValue(false);

            entity.HasOne(d => d.MaKhungGioNavigation).WithMany(p => p.LichSans).HasConstraintName("FK__LichSan__maKhung__403A8C7D");

            entity.HasOne(d => d.MaSanNavigation).WithMany(p => p.LichSans).HasConstraintName("FK__LichSan__maSan__3F466844");
        });

        modelBuilder.Entity<San>(entity =>
        {
            entity.HasKey(e => e.MaSan).HasName("PK__San__0C895661FE97EB9D");

            entity.Property(e => e.MaSan).ValueGeneratedNever();
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__5B439C430946D107");

            entity.Property(e => e.SoLuongTon).HasDefaultValue(0);

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.SanPhams)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__SanPham__maDanhM__6383C8BA");

            entity.HasMany(d => d.MaGiamGia).WithMany(p => p.MaSanPhams)
                .UsingEntity<Dictionary<string, object>>(
                    "SanPhamGiamGium",
                    r => r.HasOne<GiamGium>().WithMany()
                        .HasForeignKey("MaGiamGia")
                        .HasConstraintName("FK__SanPhamGi__maGia__6FE99F9F"),
                    l => l.HasOne<SanPham>().WithMany()
                        .HasForeignKey("MaSanPham")
                        .HasConstraintName("FK__SanPhamGi__maSan__6EF57B66"),
                    j =>
                    {
                        j.HasKey("MaSanPham", "MaGiamGia").HasName("PK__SanPhamG__94652D0122FCB248");
                        j.ToTable("SanPhamGiamGia");
                        j.IndexerProperty<int>("MaSanPham").HasColumnName("maSanPham");
                        j.IndexerProperty<int>("MaGiamGia").HasColumnName("maGiamGia");
                    });
        });

        modelBuilder.Entity<TinhNangSanPham>(entity =>
        {
            entity.HasKey(e => e.MaTinhNang).HasName("PK__TinhNang__A12681BFC2683EA1");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.TinhNangSanPhams)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__TinhNangS__maSan__0B91BA14");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__User__446439EA4B632387");

            entity.HasIndex(e => e.ResetOtp, "UQ_User_ResetOtp_NotNull")
                .IsUnique()
                .HasFilter("([ResetOtp] IS NOT NULL)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
