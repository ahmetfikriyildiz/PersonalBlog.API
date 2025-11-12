using PersonalBlog.API.Models;
using System.Security.Claims;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}

