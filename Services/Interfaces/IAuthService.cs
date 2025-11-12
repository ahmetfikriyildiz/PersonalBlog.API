using PersonalBlog.API.DTOs.Auth;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> LoginAsync(LoginDto dto);
        Task<TokenResponseDto> RegisterAsync(RegisterDto dto);
    }
}

