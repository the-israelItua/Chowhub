using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Dtos.Customers;
using ChowHub.Models;

namespace ChowHub.Mappers
{
    public static class CustomerMapper
    {
        public static CustomerDto ToCustomerDto(this Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                UserType = customer.ApplicationUser.UserType,
                Name = customer.ApplicationUser.Name,
                Address = customer.ApplicationUser.Address,
                Lga = customer.ApplicationUser.Lga,
                State = customer.ApplicationUser.State,
                CreatedAt = customer.ApplicationUser.CreatedAt,
            };
        }
    }
}