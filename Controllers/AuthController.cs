using ChowHub.Data;
using ChowHub.Dtos;
using ChowHub.Dtos.Restaurants;
using ChowHub.Interfaces;
using ChowHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChowHub.Controllers.Restaurants
{
    [ApiController]
    [Route("api/restaurant")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRestaurantRepository _restaurantRepo;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager, IRestaurantRepository restaurantRepository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _restaurantRepo = restaurantRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateRestaurantDto createRestaurantDto)
        {
            try
            {
                var existingRestaurant = await _restaurantRepo.RestaurantEmailExists(createRestaurantDto.Email);
                if (existingRestaurant)
                {
                    return Conflict(new ErrorResponse<string> { Status = 409, Message = "A restaurant with this email already exists." });
                }

                var applicationUser = new ApplicationUser
                {
                    UserName = createRestaurantDto.Email,
                    Email = createRestaurantDto.Email,
                    UserType = "Restaurant",
                    Name = createRestaurantDto.Name,
                    Address = createRestaurantDto.Address,
                    Lga = createRestaurantDto.Lga,
                    State = createRestaurantDto.State,
                };

                var createdUser = await _userManager.CreateAsync(applicationUser, createRestaurantDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(applicationUser, "Restaurant");
                    if (roleResult.Succeeded)
                    {
                        var restaurant = new Restaurant
                        {
                            ApplicationUser = applicationUser,
                            Description = createRestaurantDto.Description,
                            CuisineType = createRestaurantDto.CuisineType,
                            ImageUrl = createRestaurantDto.ImageUrl,
                            LogoUrl = createRestaurantDto.LogoUrl,
                        };

                        await _restaurantRepo.CreateAsync(restaurant);
                        var responseData = new RestaurantDto
                        {
                            Id = restaurant.Id,
                            UserType = applicationUser.UserType,
                            Name = applicationUser.Name,
                            Address = applicationUser.Address,
                            Lga = applicationUser.Lga,
                            State = applicationUser.State,
                            RestaurantId = restaurant.Id,
                            Description = restaurant.Description,
                            CuisineType = restaurant.CuisineType,
                            LogoUrl = restaurant.LogoUrl,
                            ImageUrl = restaurant.ImageUrl,
                            Rating = restaurant.Rating,
                            CreatedAt = restaurant.CreatedAt,
                            UpdatedAt = restaurant.UpdatedAt,
                            IsActive = restaurant.IsActive,
                            Status = restaurant.Status,
                        };

                        return StatusCode(201, new ApiResponse<RestaurantDto>
                        {
                            Status = 201,
                            Message = "Restaurant created successfully.",
                            Data = responseData,
                            Token = _tokenService.CreateToken(applicationUser)
                        });

                    }
                    else
                    {
                        var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();
                        return StatusCode(500, new ErrorResponse<List<string>>
                        {
                            Status = 500,
                            Message = "Failed to assign role",
                            Data = roleErrors
                        });
                    }
                }
                else
                {
                    var creationErrors = createdUser.Errors
                        .Where(e => !e.Description.Contains("Username", StringComparison.OrdinalIgnoreCase))
                        .Select(e => e.Description)
                        .ToList();
                    return StatusCode(500, new ErrorResponse<List<string>>
                    {
                        Status = 500,
                        Message = "Failed to create user",
                        Data = creationErrors
                    });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorResponse<string>
                {
                    Status = 500,
                    Message = "An unexpected error occurred",
                    Data = e.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var restaurant = await _restaurantRepo.GetByEmailAsync(loginDto.Email);
            if (restaurant == null)
            {
                return Unauthorized(new ErrorResponse<string>
                {
                    Status = 401,
                    Message = "Email or password incorrect"
                });
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(restaurant.ApplicationUser, loginDto.Password, false);

            if (!passwordCheck.Succeeded)
            {
                return Unauthorized(new ErrorResponse<string>
                {
                    Status = 401,
                    Message = "Email or password incorrect"
                });
            }


            var responseData = new RestaurantDto
            {
                Id = restaurant.Id,
                UserType = restaurant.ApplicationUser.UserType,
                Name = restaurant.ApplicationUser.Name,
                Address = restaurant.ApplicationUser.Address,
                Lga = restaurant.ApplicationUser.Lga,
                State = restaurant.ApplicationUser.State,
                RestaurantId = restaurant.Id,
                Description = restaurant.Description,
                CuisineType = restaurant.CuisineType,
                LogoUrl = restaurant.LogoUrl,
                ImageUrl = restaurant.ImageUrl,
                Rating = restaurant.Rating,
                CreatedAt = restaurant.CreatedAt,
                UpdatedAt = restaurant.UpdatedAt,
                IsActive = restaurant.IsActive,
                Status = restaurant.Status,
            };

            return StatusCode(201, new ApiResponse<RestaurantDto>
            {
                Status = 201,
                Message = "Login successfully.",
                Data = responseData,
                Token = _tokenService.CreateToken(restaurant.ApplicationUser)
            });
        }
    }
}