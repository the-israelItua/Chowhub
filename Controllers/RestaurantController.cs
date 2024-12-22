using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Data;
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
        public async Task<IActionResult> GetRestaurants([FromQuery] RestaurantQueryObject queryObject){
            var restaurants = await _restaurantRepo.GetAsync(queryObject);
            var mappedRestaurants = restaurants.Select(r => r.ToRestaurantDto()).ToList();
            return Ok(new ApiResponse<List<RestaurantDto>>{
                Status = 200,
                Message = "Restaurant fetched successfully",
                Data = mappedRestaurants
            });
        }
    }
}