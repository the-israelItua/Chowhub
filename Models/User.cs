using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Role { get; set; } = "Customer";
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Cart>? Carts { get; set; }
    }
}