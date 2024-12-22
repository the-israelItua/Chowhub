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
        private readonly UserManager<ApplicationUser> _restaurantManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDBContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> restaurantManager, ITokenService tokenService, ApplicationDBContext dbContext, SignInManager<ApplicationUser> signInManager)
        {
            _restaurantManager = restaurantManager;
            _tokenService = tokenService;
            _dbContext = dbContext;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateRestaurantDto createRestaurantDto)
        {
            try
            {
                var existingRestaurant = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.ApplicationUser.Email == createRestaurantDto.Email);
                if (existingRestaurant != null)
                {
                    return Conflict(new { message = "A restaurant with this email already exists." });
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

                var restaurant = new Restaurant
                {
                    ApplicationUser = applicationUser
                };

                var createdUser = await _restaurantManager.CreateAsync(applicationUser, createRestaurantDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _restaurantManager.AddToRoleAsync(applicationUser, "Restaurant");
                    if (roleResult.Succeeded)
                    {
                        var responseData = new NewRestaurantDto
                        {
                            User = applicationUser,
                            Token = _tokenService.CreateToken(applicationUser)
                        };

                        return Ok(new ApiResponse<NewRestaurantDto>
                        {
                            Status = 201,
                            Message = "Restaurant created successfully.",
                            Data = responseData
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
            var foundUser = await _restaurantManager.Users.FirstOrDefaultAsync(s => s.Email == loginDto.Email);
            if (foundUser == null)
            {
                return Unauthorized(new ErrorResponse<string>
                {
                    Status = 401,
                    Message = "Email or password incorrect"
                });
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(foundUser, loginDto.Password, false);

            if (!passwordCheck.Succeeded)
            {
                return Unauthorized(new ErrorResponse<string>
                {
                    Status = 401,
                    Message = "Email or password incorrect"
                });
            }


            var responseData = new NewRestaurantDto
            {
                User = foundUser,
                Token = _tokenService.CreateToken(foundUser)
            };

            return Ok(new ApiResponse<NewRestaurantDto>
            {
                Status = 201,
                Message = "Login successful.",
                Data = responseData
            });
        }
    }
}
