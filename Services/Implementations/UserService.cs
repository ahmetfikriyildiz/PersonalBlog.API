using PersonalBlog.API.DTOs.Users;
using PersonalBlog.API.Exceptions;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Interfaces;

namespace PersonalBlog.API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        // AutoMapper kullanmıyoruz, manuel mapleme yapıyoruz şimdilik projenin yapısına sadık kalarak.
        
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
                throw new NotFoundException($"User with ID {userId} not found.");

            return MapToDto(user);
        }

        public async Task<UserResponseDto> UpdateUserAsync(int userId, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
                throw new NotFoundException($"User with ID {userId} not found.");

            // Update fields if provided
            if (dto.FullName != null) user.FullName = dto.FullName;
            if (dto.Title != null) user.Title = dto.Title;
            if (dto.Bio != null) user.Bio = dto.Bio;
            if (dto.AvatarUrl != null) user.AvatarUrl = dto.AvatarUrl;
            
            // Github/Linkedin alanları User modelinde yoksa eklememiz gerekebilir veya sadece olanları güncelleriz.
            // Model kontrolü yapalım, şimdilik User modelinde ne varsa onu güncelleyelim.
            
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            return MapToDto(user);
        }

        private static UserResponseDto MapToDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Title = user.Title,
                Bio = user.Bio,
                AvatarUrl = user.AvatarUrl
            };
        }
    }
}

