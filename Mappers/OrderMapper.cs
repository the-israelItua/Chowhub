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
       public static  OrderDto ToOrderDto(this Order order){
        var customer = new CustomerDto{
            Id = order.Customer.Id,
             UserType = order.Customer.ApplicationUser.UserType,
                Name = order.Customer.ApplicationUser.Name,
                Address = order.Customer.ApplicationUser.Address,
                Lga = order.Customer.ApplicationUser.Lga,
                State = order.Customer.ApplicationUser.State,
                CreatedAt = order.Customer.ApplicationUser.CreatedAt,
        };
        return new OrderDto {
            Id = order.Id,
            TotalAmount = order.TotalAmount,
            Amount = order.Amount,
            ServiceCharge = order.ServiceCharge,
            DeliveryFee = order.DeliveryFee,
            Customer = customer,
            Restaurant = order.Restaurant.ToRestaurantDto(),
            // DriverId = order.DriverId,
            Status = order.Status,
            CreatedAt = order.CreatedAt
        };
       }
    }
}