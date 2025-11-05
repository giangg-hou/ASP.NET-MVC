using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL002.Models
{
    public class Sach
    {
        [Key]
        [Column("ma_sach")]
        [Display(Name = "Mã sách")]
        public int MaSach { get; set; }

        [Required(ErrorMessage = "Tên sách không được để trống")]
        [StringLength(255)]
        [Column("ten_sach")]
        [Display(Name = "Tên sách")]
        public string TenSach { get; set; }

        [Required(ErrorMessage = "Hãy chọn tác giả")]
        [Column("ma_tac_gia")]
        [Display(Name = "Tác giả")]
        public int? MaTacGia { get; set; }

        [Required(ErrorMessage = "Hãy chọn nhà xuất bản")]
        [Column("ma_nha_xuat_ban")]
        [Display(Name = "Nhà xuất bản")]
        public int? MaNhaXuatBan { get; set; }

        [Required(ErrorMessage = "Hãy chọn danh mục")]
        [Column("ma_danh_muc")]
        [Display(Name = "Danh mục")]
        public int? MaDanhMuc { get; set; }

        [Column("mo_ta")]
        [Display(Name = "Mô tả")]
        [DataType(DataType.MultilineText)]
        public string MoTa { get; set; }

        [Required]
        [Column("gia_ban", TypeName = "decimal(10,2)")]
        [Display(Name = "Giá bán")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán phải lớn hơn 0")]
        public decimal GiaBan { get; set; }

        [Required]
        [Column("so_luong_ton")]
        [Display(Name = "Số lượng tồn")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được âm")]
        public int SoLuongTon { get; set; }

        [StringLength(255)]
        [Column("hinh_anh")]
        [Display(Name = "Hình ảnh")]
        public string HinhAnh { get; set; }

        [Column("nam_xuat_ban")]
        [Display(Name = "Năm xuất bản")]
        [Range(1000, 9999, ErrorMessage = "Năm xuất bản không hợp lệ")]
        public int NamXuatBan { get; set; }

        [StringLength(20)]
        [Column("ngay_tao")]
        [Display(Name = "Ngày tạo")]
        public string NgayTao { get; set; }

        [Column("trang_thai")]
        [Display(Name = "Trạng thái")]
        public bool TrangThai { get; set; } = false; // false = Chờ duyệt, true = Đã duyệt

        // Navigation properties
        [ForeignKey("MaTacGia")]
        public virtual TacGia TacGia { get; set; }

        [ForeignKey("MaNhaXuatBan")]
        public virtual NhaXuatBan NhaXuatBan { get; set; }

        [ForeignKey("MaDanhMuc")]
        public virtual DanhMuc DanhMuc { get; set; }

        public virtual ICollection<GioHang> GioHangs { get; set; }
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual ICollection<DanhGia> DanhGias { get; set; }
        public virtual ICollection<YeuThich> YeuThichs { get; set; }
    }
}