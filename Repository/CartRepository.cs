using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Data;
using ChowHub.helpers;
using ChowHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ChowHub.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public CartRepository(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<List<Cart>> GetAsync(string userId)
        {
            return await _applicationDBContext.Carts.Where(s => s.UserId == userId).Include(c => c.Restaurant).ToListAsync();
        }

        public async Task<Cart?> GetByIdAsync(int id, string userId)
        {
            return await _applicationDBContext.Carts
                        .Include(c => c.Restaurant)
                        .Include(c => c.CartItems)
                        .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
        }
        public async Task<Cart> CreateAsync(Cart cart)
        {
            await _applicationDBContext.Carts.AddAsync(cart);
            await _applicationDBContext.SaveChangesAsync();
            return cart;
        }

         public async Task<Cart?> DeleteAsync(int id, string userId)
        {
            var cart = await _applicationDBContext.Carts.FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (cart == null)
            {
                return null;
            }

            _applicationDBContext.Carts.Remove(cart);
            await _applicationDBContext.SaveChangesAsync();
            return cart;
        }
    }
}