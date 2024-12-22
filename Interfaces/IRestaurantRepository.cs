using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.helpers;
using ChowHub.Models;

namespace ChowHub.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<List<Restaurant>> GetAsync(RestaurantQueryObject queryObject);
        Task<Restaurant?> GetByIdAsync(int id);
        Task<bool> RestaurantExists(int? id);
    }
}