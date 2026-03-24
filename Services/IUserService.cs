using System.Collections.Generic;
using System.Threading.Tasks;
using UserApi.DTOs;

namespace UserApi.Services;

public interface IUserService
{
    Task<List<UserResponseDto>> GetUsersAsync();
    Task<UserResponseDto> CreateUserAsync(CreateUserDto dto);
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto?> UpdateUserAsync(int id, CreateUserDto dto);
    Task<bool> DeleteUserAsync(int id);
}