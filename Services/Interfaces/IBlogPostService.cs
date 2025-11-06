using PersonalBlog.API.DTOs.BlogPost;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPostResponseDto>> GetAllPostsAsync();
        Task<IEnumerable<BlogPostResponseDto>> GetPublishedPostsAsync();
        Task<BlogPostResponseDto?> GetPostByIdAsync(int id);
        Task<BlogPostResponseDto> CreatePostAsync(CreateBlogPostDto dto);
        Task<BlogPostResponseDto> UpdatePostAsync(UpdateBlogPostDto dto);
        Task<bool> DeletePostAsync(int id);
    }
}

