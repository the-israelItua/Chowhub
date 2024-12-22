using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Data;
using ChowHub.helpers;
using ChowHub.Interfaces;
using ChowHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ChowHub.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public RestaurantRepository(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<List<Restaurant>> GetAsync(RestaurantQueryObject queryObject){
            var restaurants = _applicationDBContext.Restaurants.AsQueryable();

            if(!string.IsNullOrWhiteSpace(queryObject.Name)){
                restaurants.Where(r => r.ApplicationUser.Name.Contains(queryObject.Name));
            };
            
            var skipNumber = (queryObject.PageNumber - 1) * queryObject.PageSize;

            return await restaurants.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();

        }
        public async Task<Restaurant?> GetByIdAsync(int id){
            return await _applicationDBContext.Restaurants.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> RestaurantExists(int? id){
            return await _applicationDBContext.Restaurants.AnyAsync(s => s.Id == id);
        }
    }
}