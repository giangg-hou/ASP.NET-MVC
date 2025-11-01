using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BTL002.Models
{
    // Enum cho vai trò người dùng
    public enum VaiTro
    {
        [Display(Name = "Khách hàng")]
        KhachHang,
        Admin
    }

    // Enum cho trạng thái đơn hàng
    public enum TrangThaiDonHang
    {
        [Display(Name = "Chờ xác nhận")]
        ChoXacNhan,
        [Display(Name = "Đang giao")]
        DangGiao,
        [Display(Name = "Đã giao")]
        DaGiao,
        [Display(Name = "Đã hủy")]
        DaHuy
    }

    // Enum cho phương thức thanh toán
    public enum PhuongThucThanhToan
    {
        COD,
        [Display(Name = "Chuyển khoản")]
        ChuyenKhoan
    }
}
