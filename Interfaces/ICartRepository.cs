using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Models;

namespace ChowHub.helpers
{
    public interface ICartRepository
    {
        public Task<List<Cart>> GetAsync(string userId);
        public Task<Cart> CreateAsync(Cart cart);
        public Task<Cart?> GetByIdAsync(int id, string userId);
        public Task<Cart?> DeleteAsync(int id, string userId);
    }
}