using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL002.Models
{
    public class NhaXuatBan
    {
        [Key]
        [Column("ma_nha_xuat_ban")]
        [Display(Name = "Mã nhà xuất bản")]
        public int MaNhaXuatBan { get; set; }

        [Required(ErrorMessage = "Tên nhà xuất bản không được để trống")]
        [StringLength(200)]
        [Column("ten_nha_xuat_ban")]
        [Display(Name = "Tên nhà xuất bản")]
        public string TenNhaXuatBan { get; set; }

        [StringLength(255)]
        [Column("dia_chi")]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        // Navigation properties
        public virtual ICollection<Sach> Sachs { get; set; }
    }
}
