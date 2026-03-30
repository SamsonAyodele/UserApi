using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(TokenDto dto)
    {
        _logger.LogInformation("Refreshing token");
        try
        {
            var refresh = await _authService.RefreshTokenAsync(dto.RefreshToken);
            return Ok(new ApiResponse<Object>
            {
                Success = true,
                Message = "Token refreshed successfully",
                Data = refresh
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return BadRequest(new ApiResponse<Object>
            {
                Success = false,
                Message = ex.Message,
                Data = null
            });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(TokenDto dto)
    {
        _logger.LogInformation("Logging out user with refresh token: {RefreshToken}", dto.RefreshToken);
        try
        {
            var logout = await _authService.LogoutAsync(dto.RefreshToken);
            return Ok(new ApiResponse<Object>
            {
                Success = true,
                Message = " User logged out successfully",
                Data = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging out user with refresh token: {RefreshToken}", dto.RefreshToken);
            return BadRequest(new ApiResponse<Object>
            {
                Success = false,
                Message = ex.Message,
                Data = null
            });
        }
    }
}