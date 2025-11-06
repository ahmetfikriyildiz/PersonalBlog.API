using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.BlogPost;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;

namespace PersonalBlog.API.Repositories.Implementations
{
    public class BlogPostRepository : Repository<BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(PersonalBlogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BlogPostResponseDto>> GetAllPostsDtoAsync()
        {
            return await _context.BlogPosts
                .Where(bp => !bp.IsDeleted)
                .OrderByDescending(bp => bp.CreatedAt)
                .Select(bp => new BlogPostResponseDto
                {
                    Id = bp.Id,
                    Title = bp.Title,
                    Slug = bp.Slug,
                    Content = bp.Content,
                    IsPublished = bp.IsPublished,
                    CreatedAt = bp.CreatedAt,
                    UpdatedAt = bp.UpdatedAt,
                    AuthorName = bp.User != null ? bp.User.FullName : null
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BlogPostResponseDto>> GetPublishedPostsDtoAsync()
        {
            return await _context.BlogPosts
                .Where(bp => !bp.IsDeleted && bp.IsPublished)
                .OrderByDescending(bp => bp.CreatedAt)
                .Select(bp => new BlogPostResponseDto
                {
                    Id = bp.Id,
                    Title = bp.Title,
                    Slug = bp.Slug,
                    Content = bp.Content,
                    IsPublished = bp.IsPublished,
                    CreatedAt = bp.CreatedAt,
                    UpdatedAt = bp.UpdatedAt,
                    AuthorName = bp.User != null ? bp.User.FullName : null
                })
                .ToListAsync();
        }

        public async Task<BlogPostResponseDto?> GetPostByIdDtoAsync(int id)
        {
            return await _context.BlogPosts
                .Where(bp => bp.Id == id && !bp.IsDeleted)
                .Select(bp => new BlogPostResponseDto
                {
                    Id = bp.Id,
                    Title = bp.Title,
                    Slug = bp.Slug,
                    Content = bp.Content,
                    IsPublished = bp.IsPublished,
                    CreatedAt = bp.CreatedAt,
                    UpdatedAt = bp.UpdatedAt,
                    AuthorName = bp.User != null ? bp.User.FullName : null
                })
                .FirstOrDefaultAsync();
        }

        public async Task<BlogPost?> GetPostWithUserAsync(int id)
        {
            return await _context.BlogPosts
                .Include(bp => bp.User)
                .FirstOrDefaultAsync(bp => bp.Id == id && !bp.IsDeleted);
        }
    }
}

