using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.enums;

namespace ChowHub.Dtos.Restaurants
{
    public class UpdateRestaurantDto
    {
        public string? Description { get; set; }
        public string? CuisineType { get; set; }
        public string? LogoUrl { get; set; }
        public string? ImageUrl { get; set; }
        public RestaurantStatus Status { get; set; }
    }
}