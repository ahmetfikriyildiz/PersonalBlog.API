using PersonalBlog.API.DTOs.BlogPost;
using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Repositories.Interfaces
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        Task<IEnumerable<BlogPostResponseDto>> GetAllPostsDtoAsync();
        Task<PagedResponse<BlogPostResponseDto>> GetAllPostsDtoPagedAsync(PaginationFilter filter);
        Task<IEnumerable<BlogPostResponseDto>> GetPublishedPostsDtoAsync();
        Task<PagedResponse<BlogPostResponseDto>> GetPublishedPostsDtoPagedAsync(PaginationFilter filter);
        Task<BlogPostResponseDto?> GetPostByIdDtoAsync(int id);
        Task<BlogPost?> GetPostWithUserAsync(int id);
    }
}

