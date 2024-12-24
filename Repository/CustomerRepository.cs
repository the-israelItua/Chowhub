using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Data;
using ChowHub.Interfaces;
using ChowHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ChowHub.Repository
{
    public class CustomerRepository: ICustomerRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public CustomerRepository(ApplicationDBContext applicationDBContext)
        {
          _applicationDBContext = applicationDBContext;  
        }
        public async Task<Customer> CreateAsync(Customer customer){
            await _applicationDBContext.Customers.AddAsync(customer);
            await _applicationDBContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> GetByEmailAsync(string email){
           return await _applicationDBContext.Customers.Include(c => c.ApplicationUser).FirstOrDefaultAsync(s => s.ApplicationUser.Email == email);
        }

        public async Task<bool> CustomerEmailExists(string email){
           return await _applicationDBContext.Customers.Include(c => c.ApplicationUser).AnyAsync(s => s.ApplicationUser.Email == email);
        }
    }
}