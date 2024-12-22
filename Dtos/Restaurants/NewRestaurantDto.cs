using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Models;

namespace ChowHub.Dtos.Restaurants
{
    public class NewRestaurantDto
    {
        public ApplicationUser User { get; set; }
        public Restaurant Restaurant { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}