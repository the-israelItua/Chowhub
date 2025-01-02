using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Dtos.Customers;
using ChowHub.Dtos.Restaurants;
using ChowHub.Models;

namespace ChowHub.Dtos.Orders
{
    public class OrderDto
    { public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal DeliveryFee { get; set; }
        public int? CustomerId { get; set; }
        public CustomerDto? Customer { get; set; }
        public int? RestaurantId { get; set; }
        public RestaurantDto? Restaurant { get; set; }
        public int? DriverId { get; set; }
        // public Driver? Driver { get; set; }
        public string Status { get; set; } = "PENDING_PAYMENT";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}