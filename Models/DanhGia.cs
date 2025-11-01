using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL002.Models
{
    public class DanhGia
    {
        [Key]
        [Column("ma_danh_gia")]
        [Display(Name = "Mã đánh giá")]
        public int MaDanhGia { get; set; }

        [Required]
        [Column("ma_sach")]
        [Display(Name = "Sách")]
        public int MaSach { get; set; }

        [Required]
        [Column("ma_nguoi_dung")]
        [Display(Name = "Người dùng")]
        public int MaNguoiDung { get; set; }

        [Required]
        [Column("diem_danh_gia")]
        [Display(Name = "Điểm đánh giá")]
        [Range(1, 5, ErrorMessage = "Điểm đánh giá từ 1 đến 5")]
        public int DiemDanhGia { get; set; }

        [Column("noi_dung")]
        [Display(Name = "Nội dung")]
        [DataType(DataType.MultilineText)]
        public string NoiDung { get; set; }

        [Column("ngay_danh_gia")]
        [Display(Name = "Ngày đánh giá")]
        [StringLength(20)]
        public string NgayDanhGia { get; set; }

        // Navigation properties
        [ForeignKey("MaSach")]
        public virtual Sach Sach { get; set; }

        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung NguoiDung { get; set; }
    }
}
