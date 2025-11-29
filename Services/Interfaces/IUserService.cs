using PersonalBlog.API.DTOs.Users;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> UpdateUserAsync(int userId, UpdateUserDto dto);
        Task<UserResponseDto> GetUserByIdAsync(int userId);
    }
}

