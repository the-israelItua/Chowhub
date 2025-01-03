using ChowHub.helpers;
using ChowHub.Models;

namespace ChowHub.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<List<Restaurant>> GetAsync(RestaurantQueryObject queryObject);
        Task<Restaurant?> GetByIdAsync(int id);
        Task<Restaurant?> GetByUserIdAsync(string id);
        Task<Restaurant?> GetByEmailAsync(string email);
        Task<Restaurant> CreateAsync(Restaurant restaurant);
        Task<Restaurant> UpdateAsync(Restaurant restaurant);
        Task<bool> RestaurantEmailExists(string email);
        Task<bool> RestaurantExists(int id);
        Task<List<Order>> GetOrdersAsync(string userId, PaginationQueryObject queryObject);
        Task<Order?> GetOrderByIdAsync(int id, string userid);
        Task<Order> UpdateOrderStatusAsync(Order order);
    }
}