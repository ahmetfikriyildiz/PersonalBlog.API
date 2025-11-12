using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Auth;
using PersonalBlog.API.DTOs.Users;
using PersonalBlog.API.Exceptions;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Interfaces;

namespace PersonalBlog.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly PersonalBlogDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IRepository<User> userRepository,
            PersonalBlogDbContext context,
            IJwtService jwtService,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && !u.IsDeleted);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash))
            {
                throw new BadRequestException("Invalid email or password.");
            }

            // BCrypt ile password doğrulama
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                throw new BadRequestException("Invalid email or password.");
            }

            // Token oluştur
            var token = _jwtService.GenerateToken(user);

            return new TokenResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60), // JwtSettings'den alınabilir
                User = new UserResponseDto
                {
                    Id = user.Id,
                    FullName = user.FullName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Title = user.Title,
                    Bio = user.Bio,
                    AvatarUrl = user.AvatarUrl
                }
            };
        }

        public async Task<TokenResponseDto> RegisterAsync(RegisterDto dto)
        {
            // Email unique kontrolü
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && !u.IsDeleted);

            if (existingUser != null)
            {
                throw new ConflictException("A user with this email already exists.");
            }

            // Password hash'le
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Yeni user oluştur
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            // Token oluştur
            var token = _jwtService.GenerateToken(user);

            return new TokenResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                User = new UserResponseDto
                {
                    Id = user.Id,
                    FullName = user.FullName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Title = user.Title,
                    Bio = user.Bio,
                    AvatarUrl = user.AvatarUrl
                }
            };
        }
    }
}

