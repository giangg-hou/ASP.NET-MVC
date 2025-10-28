using BTL001.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BTL001.Models
{
    public enum UserRole
    {
        Customer = 0,
        Admin = 1
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; }

        [Required, MaxLength(150)]
        public string Email { get; set; }

        [Required, MaxLength(256)]
        public string PasswordHash { get; set; } // Lưu hash + salt, không lưu password plain

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        public UserRole Role { get; set; } = UserRole.Customer;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
