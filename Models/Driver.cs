using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChowHub.Models
{
    [Table("Drivers")]
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Lga { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string StateOfOrigin { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Role { get; set; } = "Driver";
        public string Status { get; set; } = "AVAILABLE";
        public ICollection<Order>? Orders { get; set; } 
    }
}