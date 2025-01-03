using System.Security.Claims;
using ChowHub.Dtos.Orders;
using ChowHub.Dtos.Restaurants;
using ChowHub.Enums;
using ChowHub.helpers;
using ChowHub.Interfaces;
using ChowHub.Mappers;
using ChowHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChowHub.Controllers
{
    [Authorize]
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

        [HttpPatch]
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
            if (updateDto.Status == RestaurantStatus.OPEN || updateDto.Status == RestaurantStatus.CLOSED)
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

        [HttpGet("orders")]
        public async Task<IActionResult> GetRestaurantOrders(PaginationQueryObject queryObject)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _restaurantRepo.GetOrdersAsync(userId, queryObject);
            var mappedOrders = orders.Select(r => r.ToOrderDto()).ToList();
            return Ok(new ApiResponse<List<OrderDto>>
            {
                Status = 200,
                Message = "Orders fetched successfully",
                Data = mappedOrders
            });
        }

        [HttpGet("orders/{id:int}")]
        public async Task<IActionResult> GetRestaurantOrderById([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var order = await _restaurantRepo.GetOrderByIdAsync(id, userId);

            if (order == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Order not found"
                });
            }

            return Ok(new ApiResponse<OrderDto>
            {
                Status = 200,
                Message = "Order fetched successfully",
                Data = order.ToOrderDto()
            });
        }

        [HttpPatch("orders/{id:int}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int id, UpdateRestaurantOrderDto updateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var order = await _restaurantRepo.GetOrderByIdAsync(id, userId);
            if (order == null)
            {
                return Unauthorized(new ErrorResponse<string>
                {
                    Status = 401,
                    Message = "Order not found"
                });
            }


            if (!string.IsNullOrWhiteSpace(updateDto.Status))
            {
                order.Status = updateDto.Status;
            }

            await _restaurantRepo.UpdateOrderStatusAsync(order);
            return Ok(new ApiResponse<OrderDto>
            {
                Status = 200,
                Message = "Order status updated successfully.",
                Data = order.ToOrderDto()
            });

        }

    }
}