using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Dtos.Carts;
using ChowHub.Models;

namespace ChowHub.Mappers
{
    public static class CartMapper
    {
        public static CartDto ToCartDto(this Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                RestaurantId = cart.RestaurantId,
                CartItems = cart.CartItems,
            };
        }
    }
}