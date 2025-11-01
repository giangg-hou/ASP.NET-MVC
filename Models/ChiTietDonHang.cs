using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL002.Models
{
    public class ChiTietDonHang
    {
        [Key]
        [Column("ma_chi_tiet")]
        [Display(Name = "Mã chi tiết")]
        public int MaChiTiet { get; set; }

        [Required]
        [Column("ma_don_hang")]
        [Display(Name = "Đơn hàng")]
        public int MaDonHang { get; set; }

        [Required]
        [Column("ma_sach")]
        [Display(Name = "Sách")]
        public int MaSach { get; set; }

        [Required]
        [Column("so_luong")]
        [Display(Name = "Số lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        [Required]
        [Column("gia_ban", TypeName = "decimal(10,2)")]
        [Display(Name = "Giá bán")]
        [DataType(DataType.Currency)]
        public decimal GiaBan { get; set; }

        [Required]
        [Column("thanh_tien", TypeName = "decimal(10,2)")]
        [Display(Name = "Thành tiền")]
        [DataType(DataType.Currency)]
        public decimal ThanhTien { get; set; }

        // Navigation properties
        [ForeignKey("MaDonHang")]
        public virtual DonHang DonHang { get; set; }

        [ForeignKey("MaSach")]
        public virtual Sach Sach { get; set; }
    }
}
