using PersonalBlog.API.DTOs.BlogPost;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Repositories.Interfaces
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        Task<IEnumerable<BlogPostResponseDto>> GetAllPostsDtoAsync();
        Task<IEnumerable<BlogPostResponseDto>> GetPublishedPostsDtoAsync();
        Task<BlogPostResponseDto?> GetPostByIdDtoAsync(int id);
        Task<BlogPost?> GetPostWithUserAsync(int id);
    }
}

