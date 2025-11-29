using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;

        public AuthService(
            IRepository<User> userRepository,
            PersonalBlogDbContext context,
            IJwtService jwtService,
            IEmailService emailService,
            ILogger<AuthService> logger,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
            _logger = logger;
            _configuration = configuration;
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

        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email && !u.IsDeleted);
            if (user == null)
            {
                // Security: Don't reveal if user exists or not
                return;
            }

            var token = Guid.NewGuid().ToString();
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateAsync(user);

            // Get Client URL from configuration or default to localhost:5173
            var clientUrl = _configuration["ClientSettings:Url"] ?? "http://localhost:5173";
            var resetLink = $"{clientUrl}/reset-password?token={token}";
            var subject = "Password Reset Request";
            var body = $"Please click the following link to reset your password: <a href='{resetLink}'>Reset Password</a>";

            await _emailService.SendEmailAsync(user.Email!, subject, body);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == dto.Token && !u.IsDeleted);

            if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid or expired password reset token.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);
        }
    }
}
