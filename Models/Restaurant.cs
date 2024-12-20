using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Lga { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Restaurant";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Product>? Products { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Cart>? Carts { get; set; }

    }
}