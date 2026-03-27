using Microsoft.AspNetCore.Mvc;
using UserApi.DTOs.Auth;
using UserApi.Helpers;
using UserApi.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        _logger.LogInformation("Registering a new user with email: {Email}", dto.Email);
        try
        {
            var register = await _authService.RegisterAsync(dto);
            return Ok(new ApiResponse<Object>
            {
                Success = true,
                Message = "User registered successfully",
                Data = register
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email: {Email}", dto.Email);
            return BadRequest(new ApiResponse<Object>
            {
                Success = false,
                Message = ex.Message,
                Data = null
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        _logger.LogInformation("User attempting to log in with email: {Email}", dto.Email);
        try
        {
            var login = await _authService.LoginAsync(dto);
            return Ok(new ApiResponse<Object>
            {
                Success = true,
                Message = "login successful",
                Data = login
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in user with email: {Email}", dto.Email);
            return BadRequest(new ApiResponse<Object>
            {
                Success = false,
                Message = ex.Message,
                Data = null
            });
        }
    }
}