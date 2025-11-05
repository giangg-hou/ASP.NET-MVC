using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL002.Models
{
    public class NguoiDung
    {
        [Key]
        [Column("ma_nguoi_dung")]
        [Display(Name = "Mã người dùng")]
        public int MaNguoiDung { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        [Column("ho_ten")]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Column("email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255)]
        [Column("mat_khau")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [StringLength(20)]
        [Column("so_dien_thoai")]
        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoai { get; set; }

        [StringLength(255)]
        [Column("dia_chi")]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        [Required]
        [Column("vai_tro")]
        [Display(Name = "Vai trò")]
        public VaiTro VaiTro { get; set; }

        [Required]
        [Column("trang_thai")]
        [Display(Name = "Trạng thái")]
        public TrangThaiTaiKhoan TrangThai { get; set; }

        [Column("ngay_dang_ky")]
        [Display(Name = "Ngày đăng ký")]
        [StringLength(20)]
        public string NgayDangKy { get; set; }

        // Navigation properties
        public virtual ICollection<GioHang> GioHangs { get; set; }
        public virtual ICollection<DonHang> DonHangs { get; set; }
        public virtual ICollection<DanhGia> DanhGias { get; set; }
        public virtual ICollection<YeuThich> YeuThichs { get; set; }
    }
}
