using UserApi.Application.DTOs.Auth;

namespace UserApi.Application.Interfaces;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterDto dto);
    Task<object> LoginAsync(LoginDto dto);
    Task<object?> RefreshTokenAsync(string refreshToken);
    Task<string> LogoutAsync(string refreshToken);
}