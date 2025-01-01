using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Enums;

namespace ChowHub.Dtos.Restaurants
{
    public class UpdateRestaurantDto
    {
        public string? Description { get; set; }
        public string? CuisineType { get; set; }
        public string? LogoUrl { get; set; }
        public string? ImageUrl { get; set; }
        [RegularExpression($"^({RestaurantStatus.OPEN}|{RestaurantStatus.CLOSED})$", ErrorMessage = "Invalid status. Valid values are 'OPEN' or 'CLOSED'.")]
        public string? Status { get; set; }
    }
}