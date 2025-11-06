using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.BlogPost;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PersonalBlog.API.Services.Implementations
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly PersonalBlogDbContext _context;

        public BlogPostService(IBlogPostRepository blogPostRepository, PersonalBlogDbContext context)
        {
            _blogPostRepository = blogPostRepository;
            _context = context;
        }

        public async Task<IEnumerable<BlogPostResponseDto>> GetAllPostsAsync()
        {
            return await _blogPostRepository.GetAllPostsDtoAsync();
        }

        public async Task<IEnumerable<BlogPostResponseDto>> GetPublishedPostsAsync()
        {
            return await _blogPostRepository.GetPublishedPostsDtoAsync();
        }

        public async Task<BlogPostResponseDto?> GetPostByIdAsync(int id)
        {
            return await _blogPostRepository.GetPostByIdDtoAsync(id);
        }

        public async Task<BlogPostResponseDto> CreatePostAsync(CreateBlogPostDto dto)
        {
            // Slug unique kontrolü
            var existingSlug = await _context.BlogPosts
                .AnyAsync(bp => bp.Slug == dto.Slug && !bp.IsDeleted);

            if (existingSlug)
                throw new InvalidOperationException($"A blog post with slug '{dto.Slug}' already exists.");

            // Ýlk User'ý al (þimdilik, sonra authentication ile deðiþecek)
            var user = await _context.Users.FirstOrDefaultAsync();
            if (user == null)
                throw new InvalidOperationException("No user found. Please create a user first.");

            var blogPost = new BlogPost
            {
                Title = dto.Title,
                Slug = dto.Slug,
                Content = dto.Content,
                IsPublished = dto.IsPublished,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _blogPostRepository.CreateAsync(blogPost);

            return await _blogPostRepository.GetPostByIdDtoAsync(blogPost.Id)
                ?? throw new InvalidOperationException("Failed to retrieve created blog post.");
        }

        public async Task<BlogPostResponseDto> UpdatePostAsync(UpdateBlogPostDto dto)
        {
            var blogPost = await _blogPostRepository.GetPostWithUserAsync(dto.Id);

            if (blogPost == null)
                throw new KeyNotFoundException($"Blog post with ID {dto.Id} not found.");

            if (dto.Title != null)
                blogPost.Title = dto.Title;

            if (dto.Slug != null && dto.Slug != blogPost.Slug)
            {
                var existingSlug = await _context.BlogPosts
                    .AnyAsync(bp => bp.Slug == dto.Slug && bp.Id != dto.Id && !bp.IsDeleted);

                if (existingSlug)
                    throw new InvalidOperationException($"A blog post with slug '{dto.Slug}' already exists.");

                blogPost.Slug = dto.Slug;
            }

            if (dto.Content != null)
                blogPost.Content = dto.Content;

            if (dto.IsPublished.HasValue)
                blogPost.IsPublished = dto.IsPublished.Value;

            blogPost.UpdatedAt = DateTime.UtcNow;

            await _blogPostRepository.UpdateAsync(blogPost);

            return await _blogPostRepository.GetPostByIdDtoAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Blog post with ID {dto.Id} not found after update.");
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            return await _blogPostRepository.DeleteAsync(id);
        }
    }
}

