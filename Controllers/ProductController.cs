using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Dtos.Products;
using ChowHub.helpers;
using ChowHub.Interfaces;
using ChowHub.Mappers;
using ChowHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChowHub.Controllers
{   
    [ApiController]
    [Route("api/product")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IRestaurantRepository _restaurantRepo;
        public ProductsController(IProductRepository productRepo, IRestaurantRepository restaurantRepo)
        {
          _productRepo = productRepo; 
          _restaurantRepo = restaurantRepo; 
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProducts([FromQuery] ProductsQueryObject productsQuery) {
            var products = await _productRepo.GetProductsAsync(productsQuery);
            var mappedProducts = products.Select(s => s.ToProductDto()).ToList();
            return Ok(new ApiResponse<List<ProductDto>>{
                Status = 200,
                Message = "Products fetched successfully",
                Data = mappedProducts
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductByID([FromRoute] int id){
            var product = await _productRepo.GetByIdAsync(id);

            if(product == null){
               return NotFound(new ErrorResponse<string>{
                    Status = 404,
                    Message = "Product not found"
               }); 
            }

            return Ok(new ApiResponse<ProductDto>{
                Status = 200,
                Message = "Product fetched successfully.",
                Data = product.ToProductDto()
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto){
            var restaurantExists = await _restaurantRepo.RestaurantExists(productDto.RestaurantId);
            if(!restaurantExists){
                return BadRequest(new ErrorResponse<string>{
                    Status = 403,
                    Message = "Restaurant not found"
                });
            }

            var productModel = productDto.ToProductFromCreateDto();
            await _productRepo.CreateAsync(productModel);
            return CreatedAtAction(nameof(GetProductByID), new { id = productModel.Id }, productModel.ToProductDto());
        }
    }
}