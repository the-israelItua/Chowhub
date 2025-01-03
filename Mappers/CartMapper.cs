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
                CartItems = cart.CartItems?.Select(item => new CartItemDto
                {
                    Id = item.Id,
                    Product = item.Product.ToProductDto(),
                    Quantity = item.Quantity,
                    CartId = item.CartId,
                }).ToList()
            };
        }

        public static CartItemDto ToCartItemDto(this CartItem cartItem)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                Product = cartItem.Product.ToProductDto(),
                Quantity = cartItem.Quantity,
            };
        }
    }
}