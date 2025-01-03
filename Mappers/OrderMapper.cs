using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Dtos.Customers;
using ChowHub.Dtos.Orders;
using ChowHub.Models;

namespace ChowHub.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToOrderDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                Amount = order.Amount,
                ServiceCharge = order.ServiceCharge,
                DeliveryFee = order.DeliveryFee,
                CustomerId = order.CustomerId,
                Customer = order.Customer.ToCustomerDto(),
                RestaurantId = order.RestaurantId,
                Restaurant = order.Restaurant.ToRestaurantDto(),
                DriverId = order.DriverId,
                Status = order.Status,
                CreatedAt = order.CreatedAt
            };
        }
    }
}