using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL002.Models
{
    public class YeuThich
    {
        [Key]
        [Column("ma_yeu_thich")]
        [Display(Name = "Mã yêu thích")]
        public int MaYeuThich { get; set; }

        [Required]
        [Column("ma_nguoi_dung")]
        [Display(Name = "Người dùng")]
        public int MaNguoiDung { get; set; }

        [Required]
        [Column("ma_sach")]
        [Display(Name = "Sách")]
        public int MaSach { get; set; }

        [Column("ngay_them")]
        [Display(Name = "Ngày thêm")]
        [StringLength(20)]
        public string NgayThem { get; set; }

        // Navigation properties
        [ForeignKey("MaNguoiDung")]
        public virtual NguoiDung NguoiDung { get; set; }

        [ForeignKey("MaSach")]
        public virtual Sach Sach { get; set; }
    }
}
