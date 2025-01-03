using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Dtos.Products;
using ChowHub.Models;

namespace ChowHub.Dtos.Carts
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int? CartId { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
    }
}