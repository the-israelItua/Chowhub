using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.Dtos.Carts
{
    public class CreateCartDto
    {
        [Required]
        public int RestaurantId { get; set; } 
    }
}