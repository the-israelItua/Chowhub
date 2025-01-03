using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.helpers
{
    public class ProductsQueryObject : PaginationQueryObject
    {
        public string? Name { get; set; } = string.Empty;
        [Required]
        public int RestaurantId { get; set; }
    }
}