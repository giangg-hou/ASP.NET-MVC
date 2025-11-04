using BTL002.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BTL002.Data
{
    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options): base(options){}

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<TacGia> TacGias { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public DbSet<Sach> Sachs { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<YeuThich> YeuThichs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình cho bảng NguoiDung
            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaNguoiDung);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.VaiTro)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                // declare max length for date strings explicitly
                entity.Property(e => e.NgayDangKy).HasMaxLength(20);
            });

            // Cấu hình cho bảng DanhMuc
            modelBuilder.Entity<DanhMuc>(entity =>
            {
                entity.HasKey(e => e.MaDanhMuc);
                entity.Property(e => e.TenDanhMuc).IsRequired();
            });

            // Cấu hình cho bảng TacGia
            modelBuilder.Entity<TacGia>(entity =>
            {
                entity.HasKey(e => e.MaTacGia);
                entity.Property(e => e.TenTacGia).IsRequired();
            });

            // Cấu hình cho bảng NhaXuatBan
            modelBuilder.Entity<NhaXuatBan>(entity =>
            {
                entity.HasKey(e => e.MaNhaXuatBan);
                entity.Property(e => e.TenNhaXuatBan).IsRequired();
            });

            // Cấu hình cho bảng Sach
            modelBuilder.Entity<Sach>(entity =>
            {
                entity.HasKey(e => e.MaSach);
                entity.Property(e => e.TenSach).IsRequired();
                entity.Property(e => e.GiaBan).HasPrecision(10, 2);
                entity.Property(e => e.NgayTao).HasMaxLength(20);

                entity.HasOne(e => e.TacGia)
                    .WithMany(t => t.Sachs)
                    .HasForeignKey(e => e.MaTacGia)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.NhaXuatBan)
                    .WithMany(n => n.Sachs)
                    .HasForeignKey(e => e.MaNhaXuatBan)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.DanhMuc)
                    .WithMany(d => d.Sachs)
                    .HasForeignKey(e => e.MaDanhMuc)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Cấu hình cho bảng GioHang
            modelBuilder.Entity<GioHang>(entity =>
            {
                entity.HasKey(e => e.MaGioHang);

                entity.Property(e => e.NgayThem).HasMaxLength(20);

                entity.HasOne(e => e.NguoiDung)
                    .WithMany(n => n.GioHangs)
                    .HasForeignKey(e => e.MaNguoiDung)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Sach)
                    .WithMany(s => s.GioHangs)
                    .HasForeignKey(e => e.MaSach)
                    .OnDelete(DeleteBehavior.Cascade);

                // Unique constraint: một người dùng chỉ có một dòng cho mỗi sách
                entity.HasIndex(e => new { e.MaNguoiDung, e.MaSach }).IsUnique();
            });

            // Cấu hình cho bảng DonHang
            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.MaDonHang);
                entity.Property(e => e.TongTien).HasPrecision(10, 2);
                entity.Property(e => e.TrangThai)
                    .HasConversion<string>()
                    .HasMaxLength(50);
                entity.Property(e => e.PhuongThucThanhToan)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                entity.Property(e => e.NgayDatHang).HasMaxLength(20);

                entity.HasOne(e => e.NguoiDung)
                    .WithMany(n => n.DonHangs)
                    .HasForeignKey(e => e.MaNguoiDung)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Cấu hình cho bảng ChiTietDonHang
            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => e.MaChiTiet);
                entity.Property(e => e.GiaBan).HasPrecision(10, 2);
                entity.Property(e => e.ThanhTien).HasPrecision(10, 2);

                entity.HasOne(e => e.DonHang)
                    .WithMany(d => d.ChiTietDonHangs)
                    .HasForeignKey(e => e.MaDonHang)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Sach)
                    .WithMany(s => s.ChiTietDonHangs)
                    .HasForeignKey(e => e.MaSach)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Cấu hình cho bảng DanhGia
            modelBuilder.Entity<DanhGia>(entity =>
            {
                entity.HasKey(e => e.MaDanhGia);

                entity.Property(e => e.NgayDanhGia).HasMaxLength(20);

                entity.HasOne(e => e.Sach)
                    .WithMany(s => s.DanhGias)
                    .HasForeignKey(e => e.MaSach)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.NguoiDung)
                    .WithMany(n => n.DanhGias)
                    .HasForeignKey(e => e.MaNguoiDung)
                    .OnDelete(DeleteBehavior.Cascade);

                // Một người chỉ đánh giá một lần cho mỗi sách
                entity.HasIndex(e => new { e.MaSach, e.MaNguoiDung }).IsUnique();
            });

            // Cấu hình cho bảng YeuThich
            modelBuilder.Entity<YeuThich>(entity =>
            {
                entity.HasKey(e => e.MaYeuThich);

                entity.Property(e => e.NgayThem).HasMaxLength(20);

                entity.HasOne(e => e.NguoiDung)
                    .WithMany(n => n.YeuThichs)
                    .HasForeignKey(e => e.MaNguoiDung)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Sach)
                    .WithMany(s => s.YeuThichs)
                    .HasForeignKey(e => e.MaSach)
                    .OnDelete(DeleteBehavior.Cascade);

                // Unique constraint: một người không thể yêu thích cùng một sách nhiều lần
                entity.HasIndex(e => new { e.MaNguoiDung, e.MaSach }).IsUnique();
            });

            // Seed dữ liệu mẫu
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Helper to hash a seed password using the same SHA256 logic used by AccountController
            static string HashSeedPassword(string plain)
            {
                using var sha256 = SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(plain);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }

            // Seed Danh mục
            modelBuilder.Entity<DanhMuc>().HasData(
                new DanhMuc { MaDanhMuc = 1, TenDanhMuc = "Văn học", MoTa = "Sách văn học trong nước và nước ngoài" },
                new DanhMuc { MaDanhMuc = 2, TenDanhMuc = "Kinh tế", MoTa = "Sách về kinh doanh và kinh tế" },
                new DanhMuc { MaDanhMuc = 3, TenDanhMuc = "Công nghệ", MoTa = "Sách về lập trình và công nghệ thông tin" },
                new DanhMuc { MaDanhMuc = 4, TenDanhMuc = "Tâm lý - Kỹ năng sống", MoTa = "Sách phát triển bản thân" },
                new DanhMuc { MaDanhMuc = 5, TenDanhMuc = "Thiếu nhi", MoTa = "Sách dành cho trẻ em" }
            );

            // Seed Tác giả
            modelBuilder.Entity<TacGia>().HasData(
                new TacGia { MaTacGia = 1, TenTacGia = "giang 01", TieuSu = "Nhà văn nổi tiếng Việt Nam" },
                new TacGia { MaTacGia = 2, TenTacGia = "giang 02", TieuSu = "Tác giả người Mỹ về phát triển bản thân" },
                new TacGia { MaTacGia = 3, TenTacGia = "giang 03", TieuSu = "Doanh nhân và tác giả người Mỹ" },
                new TacGia { MaTacGia = 4, TenTacGia = "giang 04", TieuSu = "Nhà văn người Brazil" },
                new TacGia { MaTacGia = 5, TenTacGia = "giang 05", TieuSu = "Nhà văn Nhật Bản đương đại" }
            );

            // Seed Nhà xuất bản
            modelBuilder.Entity<NhaXuatBan>().HasData(
                new NhaXuatBan { MaNhaXuatBan = 1, TenNhaXuatBan = "NXB Trẻ", DiaChi = "161B Lý Chính Thắng, Q.3, TP.HCM" },
                new NhaXuatBan { MaNhaXuatBan = 2, TenNhaXuatBan = "NXB Kim Đồng", DiaChi = "55 Quang Trung, Hai Bà Trưng, Hà Nội" },
                new NhaXuatBan { MaNhaXuatBan = 3, TenNhaXuatBan = "NXB DH Mở Hà Nội", DiaChi = "96 Định Công, Hoàng Mai, Hà Nội" },
                new NhaXuatBan { MaNhaXuatBan = 4, TenNhaXuatBan = "NXB Lao Động", DiaChi = "175 Giảng Võ, Đống Đa, Hà Nội" },
                new NhaXuatBan { MaNhaXuatBan = 5, TenNhaXuatBan = "NXB Thế Giới", DiaChi = "46 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội" }
            );

            // Seed User mặc định (Admin) — password hashed to match AccountController's SHA256 Verify
            var adminHashed = HashSeedPassword("123123");
            modelBuilder.Entity<NguoiDung>().HasData(
                new NguoiDung
                {
                    MaNguoiDung = 1,
                    HoTen = "Quản trị viên",
                    Email = "admin@bookstore.com",
                    MatKhau = adminHashed,
                    VaiTro = VaiTro.Admin,
                    NgayDangKy = "2025-11-05",
                    DiaChi = "Việt Nam",
                    SoDienThoai = "0000000000"
                }
            );
        }
    }
}