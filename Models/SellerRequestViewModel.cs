using Microsoft.AspNetCore.Mvc;

namespace BTL002.Models
{
    public class SellerRequestViewModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string CreatedAt { get; set; }
        public TrangThaiTaiKhoan Status { get; set; }
    }
}
