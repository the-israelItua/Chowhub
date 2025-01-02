using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Models;

namespace ChowHub.Dtos.Carts
{
    public class CartDto
    {
        public int Id { get; set; }
        public int? RestaurantId { get; set; }
        public ICollection<CartItemDto>? CartItems { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}