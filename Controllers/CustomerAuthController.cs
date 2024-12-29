using ChowHub.Data;
using ChowHub.Dtos;
using ChowHub.Dtos.Customers;
using ChowHub.Interfaces;
using ChowHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChowHub.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICustomerRepository _customerRepo;

        public CustomerAuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager, ICustomerRepository customerRepo)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _customerRepo = customerRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateCustomerDto createCustomerDto)
        {
            try
            {
                var existingCustomer = await _customerRepo.CustomerEmailExists(createCustomerDto.Email);
                if (existingCustomer)
                {
                    return Conflict(new ErrorResponse<string> { Status = 409, Message = "A customer with this email already exists." });
                }

                var applicationUser = new ApplicationUser
                {
                    UserName = createCustomerDto.Email,
                    Email = createCustomerDto.Email,
                    UserType = "Customer",
                    Name = createCustomerDto.Name,
                    Address = createCustomerDto.Address,
                    Lga = createCustomerDto.Lga,
                    State = createCustomerDto.State,
                };

                var createdUser = await _userManager.CreateAsync(applicationUser, createCustomerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(applicationUser, "CUSTOMER");
                    if (roleResult.Succeeded)
                    {
                        var customer = new Customer
                        {
                            ApplicationUser = applicationUser,
                        };

                        await _customerRepo.CreateAsync(customer);
                        var responseData = new CustomerDto
                        {
                            Id = customer.Id,
                            UserType = applicationUser.UserType,
                            Name = applicationUser.Name,
                            Address = applicationUser.Address,
                            Lga = applicationUser.Lga,
                            State = applicationUser.State,
                            CreatedAt = customer.ApplicationUser.CreatedAt,
                        };

                        return StatusCode(201, new ApiResponse<CustomerDto>
                        {
                            Status = 201,
                            Message = "customer created successfully.",
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
            var customer = await _customerRepo.GetByEmailAsync(loginDto.Email);
            if (customer == null)
            {
                return Unauthorized(new ErrorResponse<string>
                {
                    Status = 401,
                    Message = "Email or password incorrect"
                });
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(customer.ApplicationUser, loginDto.Password, false);

            if (!passwordCheck.Succeeded)
            {
                return Unauthorized(new ErrorResponse<string>
                {
                    Status = 401,
                    Message = "Email or password incorrect"
                });
            }


            var responseData = new CustomerDto
            {
                Id = customer.Id,
                UserType = customer.ApplicationUser.UserType,
                Name = customer.ApplicationUser.Name,
                Address = customer.ApplicationUser.Address,
                Lga = customer.ApplicationUser.Lga,
                State = customer.ApplicationUser.State,
                CreatedAt = customer.ApplicationUser.CreatedAt,
            };

            return StatusCode(201, new ApiResponse<CustomerDto>
            {
                Status = 201,
                Message = "Login successfully.",
                Data = responseData,
                Token = _tokenService.CreateToken(customer.ApplicationUser)
            });
        }
    }
}
