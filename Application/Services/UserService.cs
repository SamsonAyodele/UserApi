using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserApi.Infrastructure.Data;
using UserApi.Domain.Entities;
using UserApi.Application.DTOs;
using UserApi.Application.Interfaces;
using UserApi.Infrastructure.Repositories;

namespace UserApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;

    public UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<UserResponseDto>> GetUsersAsync()
    {
        _logger.LogInformation("Retrieving all users");
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email
        }).ToList();
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
    {
        _logger.LogInformation("Creating a new user with email: {Email}", dto.Email);
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

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
        var user = await _unitOfWork.Users.GetByIdAsync(id);
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
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {Id}", id);
            return null;
        }
        user.Name = dto.Name;
        user.Email = dto.Email;
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
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
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {Id}", id);
            return false;
        }
        await _unitOfWork.Users.DeleteAsync(user);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Deleted user with ID: {Id}", id);
        return true;
    }
}