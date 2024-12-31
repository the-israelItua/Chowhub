using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChowHub.Dtos.Carts;
using ChowHub.helpers;
using ChowHub.Interfaces;
using ChowHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChowHub.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/cart")]
    public class CartController : ControllerBase
    {

        private readonly ICartRepository _cartRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IProductRepository _productRepo;
        public CartController(ICartRepository cartRepo, IRestaurantRepository restaurantRepo, ICustomerRepository customerRepo, IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _restaurantRepo = restaurantRepo;
            _customerRepo = customerRepo;
            _productRepo = productRepo;
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
        public async Task<IActionResult> CreateCart([FromBody] CreateCartDto createDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            var restaurant = await _restaurantRepo.GetByIdAsync(createDto.RestaurantId);

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
                RestaurantId = createDto.RestaurantId,
            };

            await _cartRepo.CreateAsync(cartModel);

            var response = new ApiResponse<Cart>
            {
                Status = 201,
                Message = "Cart created successfully",
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
    

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemDto addCartItemDto) {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _cartRepo.GetByIdAsync(addCartItemDto.CartId, userId);

            if (cart == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Cart not found."
                });
            }

            var product = await _productRepo.GetByIdAsync(addCartItemDto.ProductId);

             if (product == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Product not found."
                });
            }

            var cartItemModel = new CartItem
            {
                CartId = addCartItemDto.CartId,
                ProductId = addCartItemDto.ProductId,
                Quantity = addCartItemDto.Quantity,
            };

            await _cartRepo.AddItemAsync(cartItemModel);

            var response = new ApiResponse<CartItem>
            {
                Status = 201,
                Message = "Product added to cart",
                Data = cartItemModel,
            };
            return CreatedAtAction(
                        nameof(GetCartById),
                        new { id = cartItemModel.Id },
                        response
                    );

    }
    
    [HttpDelete("{cartId:int}/{cartItemId:int}")]
    public async Task<IActionResult> RemoveItem([FromRoute] int cartId, int cartItemId) {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _cartRepo.GetByIdAsync(cartId, userId);

            if (cart == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Cart not found."
                });
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Item not in cart."
                });
            }

            await _cartRepo.RemoveItemAsync(cartItem);     

            return Ok(new ApiResponse<string>
            {
                Status = 201,
                Message = "Item removed from cart"
            });
    }

     [HttpDelete("{cartId:int}/{cartItemId:int}")]
    public async Task<IActionResult> UpdateQuantity([FromRoute] int cartId, int cartItemId, [FromBody] UpdateCartItemQuantityDto quantityDto) {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cart = await _cartRepo.GetByIdAsync(cartId, userId);

         if (cart == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Cart not found."
                });
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem == null)
            {
                return NotFound(new ErrorResponse<string>
                {
                    Status = 404,
                    Message = "Item not in cart."
                });
            }

        cartItem.Quantity = quantityDto.Quantity;

        await _cartRepo.UpdateItemAsync(cartItem);     

        return Ok(new ApiResponse<CartItem>
            {
                Status = 201,
                Message = "Quantity updated",
                Data = cartItem
            });
    }
    }
}