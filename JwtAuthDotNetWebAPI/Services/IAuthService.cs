using JwtAuthDotNetWebAPI.Entities;
using JwtAuthDotNetWebAPI.Models;

namespace JwtAuthDotNetWebAPI.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshtokenAsync(RefreshTokenRequestDto request);
    }
}
