using ChowHub.Dtos.Restaurants;
using ChowHub.helpers;
using ChowHub.Interfaces;
using ChowHub.Mappers;
using ChowHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChowHub.Controllers
{
    [ApiController]
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepo;
        public RestaurantController(IRestaurantRepository restaurantRepo)
        {
            _restaurantRepo = restaurantRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetRestaurants([FromQuery] RestaurantQueryObject queryObject)
        {
            var restaurants = await _restaurantRepo.GetAsync(queryObject);
            var mappedRestaurants = restaurants.Select(r => r.ToRestaurantDto()).ToList();
            return Ok(new ApiResponse<List<RestaurantDto>>
            {
                Status = 200,
                Message = "Restaurant fetched successfully",
                Data = mappedRestaurants
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRestaurantByID([FromRoute] int id)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(id);

            if (restaurant == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Restaurant not found"
                });
            }

            return Ok(new ApiResponse<RestaurantDto>
            {
                Status = 200,
                Message = "Restaurant fetched successfully.",
                Data = restaurant.ToRestaurantDto()
            });
        }
    }
}