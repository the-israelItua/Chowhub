using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Enums;

namespace ChowHub.Dtos.Restaurants
{
    public class UpdateRestaurantOrderDto
    {
        [RegularExpression("^(AcceptedByRestaurant|DeclinedByRestaurant|OrderPrepared)$",
            ErrorMessage = "Invalid status. Valid values are 'AcceptedByRestaurant', 'DeclinedByRestaurant', or 'OrderPrepared'.")]
        public string? Status { get; set; }
    }
}