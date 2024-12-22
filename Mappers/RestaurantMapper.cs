using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Dtos.Restaurants;
using ChowHub.Models;

namespace ChowHub.Mappers
{
    public static class RestaurantMapper
    {
        public static RestaurantDto ToRestaurantDto(this Restaurant restaurant){
            return new RestaurantDto{
                Id = restaurant.Id,
                Name = restaurant.ApplicationUser.Name,
                Address = restaurant.ApplicationUser.Address,
                Lga = restaurant.ApplicationUser.Lga,
                State = restaurant.ApplicationUser.State,
                CreatedAt = restaurant.ApplicationUser.CreatedAt,
            };
        }
    }
}