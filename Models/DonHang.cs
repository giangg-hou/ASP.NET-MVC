using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTL002.Models;

namespace BTL002.Models
{
    public class DonHang
    {
        [Key]
        [Column("ma_don_hang")]
        [Display(Name = "Mã đơn hàng")]
        public int MaDonHang { get; set; }

        [Required]
        [Column("ma_nguoi_dung")]
        [Display(Name = "Người dùng")]
        public int MaNguoiDung { get; set; }

        [Required]
        [Column("tong_tien", TypeName = "decimal(10,2)")]
        [Display(Name = "Tổng tiền")]
        [DataType(DataType.Currency)]
        public decimal TongTien { get; set; }

        [Required]
        [Column("trang_thai")]
        [Display(Name = "Trạng thái")]
        public TrangThaiDonHang TrangThai { get; set; }

        [Required]
        [Column("phuong_thuc_thanh_toan")]
        [Display(Name = "Phương thức thanh toán")]
        public PhuongThucThanhToan PhuongThucThanhToan { get; set; }

        [Column("ngay_dat_hang")]
        [Display(Name = "Ngày đặt hàng")] 
        [StringLength(20)]
        public string NgayDatHang { get; set; }

        // Navigation properties
        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung NguoiDung { get; set; }

        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}
