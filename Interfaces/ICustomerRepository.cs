using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Models;

namespace ChowHub.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<Customer> CreateAsync(Customer customer);
        public Task<Customer?> GetByEmailAsync(string email);
        public Task<bool> CustomerEmailExists(string email);
    }
}