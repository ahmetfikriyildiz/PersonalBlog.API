using PersonalBlog.API.DTOs.BlogPost;
using PersonalBlog.API.DTOs.Commons;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPostResponseDto>> GetAllPostsAsync();
        Task<PagedResponse<BlogPostResponseDto>> GetAllPostsPagedAsync(PaginationFilter filter);
        Task<IEnumerable<BlogPostResponseDto>> GetPublishedPostsAsync();
        Task<PagedResponse<BlogPostResponseDto>> GetPublishedPostsPagedAsync(PaginationFilter filter);
        Task<BlogPostResponseDto?> GetPostByIdAsync(int id);
        Task<BlogPostResponseDto> CreatePostAsync(CreateBlogPostDto dto);
        Task<BlogPostResponseDto> UpdatePostAsync(UpdateBlogPostDto dto);
        Task<bool> DeletePostAsync(int id);
    }
}

