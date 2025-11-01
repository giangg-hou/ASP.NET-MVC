using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL002.Models
{
    public class DanhMuc
    {
        [Key]
        [Column("ma_danh_muc")]
        [Display(Name = "Mã danh mục")]
        public int MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        [Column("ten_danh_muc")]
        [Display(Name = "Tên danh mục")]
        public string TenDanhMuc { get; set; }

        [Column("mo_ta")]
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string MoTa { get; set; }

        // Navigation properties
        public virtual ICollection<Sach> Sachs { get; set; }
    }
}
