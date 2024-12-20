using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.Models
{
    [Table("Carts")]
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Customer? User { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}