using BTL002.Models;
using System.Security.Cryptography;
using System.Text;

namespace BTL002.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BookStoreDbContext context)
        {
            context.Database.EnsureCreated();

            // Kiểm tra đã có admin chưa
            if (context.NguoiDungs.Any(u => u.VaiTro == VaiTro.Admin))
            {
                return; // DB đã có dữ liệu
            }

            // Tạo admin user
            var adminPassword = HashPassword("Admin@123");
            var admin = new NguoiDung
            {
                HoTen = "Administrator",
                Email = "admin@bookstore.com",
                MatKhau = adminPassword,
                SoDienThoai = "0123456789",
                DiaChi = "Hà Nội",
                VaiTro = VaiTro.Admin,
                NgayDangKy = DateTime.Now.ToString("yyyy-MM-dd")
            };

            context.NguoiDungs.Add(admin);
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}