using System.Security.Claims;
using ChowHub.Dtos.Restaurants;
using ChowHub.helpers;
using ChowHub.Interfaces;
using ChowHub.Mappers;
using ChowHub.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [Authorize]
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

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantDto updateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var restaurant = await _restaurantRepo.GetByUserIdAsync(userId);
            if (restaurant == null)
            {
                return Unauthorized(new ErrorResponse<string>
                {
                    Status = 401,
                    Message = "You do not have permission to perform this operation."
                });
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Description))
            {
                restaurant.Description = updateDto.Description;
            }
            if (!string.IsNullOrWhiteSpace(updateDto.CuisineType))
            {
                restaurant.CuisineType = updateDto.CuisineType;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.LogoUrl))
            {
                restaurant.LogoUrl = updateDto.LogoUrl;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.ImageUrl))
            {
                restaurant.ImageUrl = updateDto.ImageUrl;
            }
            if (updateDto.Status == enums.RestaurantStatus.OPEN || updateDto.Status == enums.RestaurantStatus.CLOSED)
            {
                restaurant.Status = updateDto.Status;
            }


            await _restaurantRepo.UpdateAsync(restaurant);
            return Ok(new ApiResponse<RestaurantDto>
            {
                Status = 200,
                Message = "Restaurant updated successfully.",
                Data = restaurant.ToRestaurantDto()
            });

        }
    }
}