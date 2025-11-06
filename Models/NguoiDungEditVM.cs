using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BTL002.Models
{
    public class NguoiDungEditVM
    {
        public int MaNguoiDung { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [StringLength(255)]
        public string? DiaChi { get; set; }

        [Required]
        public VaiTro VaiTro { get; set; }

        [Required]
        public TrangThaiTaiKhoan TrangThai { get; set; }
    }
}
