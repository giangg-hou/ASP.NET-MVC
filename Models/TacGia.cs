using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL002.Models
{
    public class TacGia
    {
        [Key]
        [Column("ma_tac_gia")]
        [Display(Name = "Mã tác giả")]
        public int MaTacGia { get; set; }

        [Required(ErrorMessage = "Tên tác giả không được để trống")]
        [StringLength(100)]
        [Column("ten_tac_gia")]
        [Display(Name = "Tên tác giả")]
        public string TenTacGia { get; set; }

        [Column("tieu_su")]
        [Display(Name = "Tiểu sử")]
        [DataType(DataType.MultilineText)]
        public string TieuSu { get; set; }

        // Navigation properties
        public virtual ICollection<Sach> Sachs { get; set; }
    }
}
