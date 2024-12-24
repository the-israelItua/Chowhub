using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }
        public string Status { get; set; } = "PENDING";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}