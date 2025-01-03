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
        public string LicenseNumber { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser? ApplicationUser { get; set; }
        public string StateOfOrigin { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string Status { get; set; } = "AVAILABLE";
        public decimal? Rating { get; set; } = 5;
        public ICollection<Order>? Orders { get; set; }
    }
}