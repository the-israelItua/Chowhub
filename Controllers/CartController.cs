using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChowHub.helpers;
using ChowHub.Interfaces;
using ChowHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChowHub.Controllers
{
    [Authorize(Roles = "CUSTOMER")]
    [ApiController]
    [Route("/cart")]
    public class CartController : ControllerBase
    {

        private readonly ICartRepository _cartRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly ICustomerRepository _customerRepo;
        public CartController(ICartRepository cartRepo, IRestaurantRepository restaurantRepo, ICustomerRepository customerRepo)
        {
            _cartRepo = cartRepo;
            _restaurantRepo = restaurantRepo;
            _customerRepo = customerRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarts(){
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var carts = await _cartRepo.GetAsync(userId);
            return Ok(new ApiResponse<List<Cart>>{
                Status = 200,
                Message = "Carts fetched successfully",
                Data = carts
            });
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCartById([FromRoute] int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var cart = await _cartRepo.GetByIdAsync(id, userId);
            if (cart == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "cart not found"
                });
            }
            return Ok(new ApiResponse<Cart>
            {
                Status = 200,
                Message = "Cart fetched successfully.",
                Data = cart
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] int restaurantId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);

            if (restaurant == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Restaurant not found"
                });
            }

            var customer = await _customerRepo.GetByEmailAsync(userEmail);

            if (customer == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Customer not found"
                });
            }

            var cartModel = new Cart
            {
                CustomerId = customer.Id,
                RestaurantId = restaurantId,
            };

            await _cartRepo.CreateAsync(cartModel);

            var response = new ApiResponse<Cart>
            {
                Status = 201,
                Message = "Product created successfully",
                Data = cartModel,
            };
            return CreatedAtAction(
                        nameof(GetCartById),
                        new { id = cartModel.Id },
                        response
                    );
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteCart([FromRoute] int id)
        {
           var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _cartRepo.DeleteAsync(id, userId);

            if (cart == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Cart not found."
                });
            }

            return NoContent();
        }
    }
}