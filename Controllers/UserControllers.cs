using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;
using UserApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using UserApi.Application.Services;
using UserApi.Application.DTOs;

namespace UserApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        _logger.LogInformation("Retrieving all users");
        var users = await _userService.GetUsersAsync();
     
        return Ok(new ApiResponse<Object>(true, "Users retrieved successfully", users));
    }


    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        _logger.LogInformation("Creating a new user with email: {Email}", dto.Email);
        var user = await _userService.CreateUserAsync(dto);
        var response = new ApiResponse<Object>(true, "User created successfully", user);
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        _logger.LogInformation("Retrieving user with ID: {Id}", id);
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found");
            return NotFound(new ApiResponse<Object>(false, "User not found", null));
        }

        return Ok(new ApiResponse<Object>(true, "User retrieved successfully", user));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, CreateUserDto dto)
    {
        _logger.LogInformation("Updating user with ID: {Id}", id);
        var user = await _userService.UpdateUserAsync(id, dto);
        if (user == null)
        {
            _logger.LogWarning("User not found");
            return NotFound(new ApiResponse<Object>(false, "User not found", null));
        }
        return Ok(new ApiResponse<Object>(true, "User updated successfully", user));
    }

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        _logger.LogInformation("Deleting user with ID: {Id}", id);
        var success = await _userService.DeleteUserAsync(id);
        if (!success)
        {
            _logger.LogWarning("User not found");
            return NotFound(new ApiResponse<Object>(false, "User not found", null));
        }
        _logger.LogInformation("Deleted user with ID: {Id}", id);
        return Ok(new ApiResponse<Object>(true, "User deleted successfully", null));
    }
}