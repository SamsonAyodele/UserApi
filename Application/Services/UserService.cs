using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserApi.Infrastructure.Data;
using UserApi.Domain.Entities;
using UserApi.Application.DTOs;

namespace UserApi.Application.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(AppDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<UserResponseDto>> GetUsersAsync()
    {
        _logger.LogInformation("Retrieving all users");
        return await _context.Users
            .Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            })
            .ToListAsync();
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
    {
        _logger.LogInformation("Creating a new user with email: {Email}", dto.Email);
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving user with ID: {Id}", id);
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {Id}", id);
            return null;
        }
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponseDto?> UpdateUserAsync(int id, CreateUserDto dto)
    {
        _logger.LogInformation("Updating user with ID: {Id}", id);
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {Id}", id);
            return null;
        }
        user.Name = dto.Name;
        user.Email = dto.Email;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        _logger.LogInformation("Deleting user with ID: {Id}", id);
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {Id}", id);
            return false;
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted user with ID: {Id}", id);
        return true;
    }
}