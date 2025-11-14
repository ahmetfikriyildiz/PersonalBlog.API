    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PersonalBlog.API.DTOs.BlogPost;
    using PersonalBlog.API.Services.Interfaces;

    namespace PersonalBlog.API.Controllers
    {
        /// <summary>
        /// Controller for managing blog posts
        /// </summary>
        [ApiController]
        [Route("api/[controller]")]
        [Produces("application/json")]
        public class BlogPostsController : ControllerBase
        {
            private readonly IBlogPostService _blogPostService;

            public BlogPostsController(IBlogPostService blogPostService)
            {
                _blogPostService = blogPostService;
            }

            /// <summary>
            /// Get all blog posts including unpublished (Admin endpoint - Requires authentication)
            /// </summary>
            [HttpGet]
            [Authorize]
            [ProducesResponseType(typeof(IEnumerable<BlogPostResponseDto>), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            public async Task<ActionResult<IEnumerable<BlogPostResponseDto>>> GetAllPosts()
            {
                var posts = await _blogPostService.GetAllPostsAsync();
                return Ok(posts);
            }

            /// <summary>
            /// Get only published blog posts (Public endpoint)
            /// </summary>
            [HttpGet("published")]
            [ProducesResponseType(typeof(IEnumerable<BlogPostResponseDto>), StatusCodes.Status200OK)]
            public async Task<ActionResult<IEnumerable<BlogPostResponseDto>>> GetPublishedPosts()
            {
                var posts = await _blogPostService.GetPublishedPostsAsync();
                return Ok(posts);
            }

            /// <summary>
            /// Get blog post by ID
            /// </summary>
            [HttpGet("{id}")]
            [ProducesResponseType(typeof(BlogPostResponseDto), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            public async Task<ActionResult<BlogPostResponseDto>> GetPostById(int id)
            {
                var post = await _blogPostService.GetPostByIdAsync(id);
                if (post == null)
                    return NotFound(new { message = $"Blog post with ID {id} not found." });
                return Ok(post);
            }

            /// <summary>
            /// Create a new blog post (Admin endpoint - Requires authentication)
            /// </summary>
            [HttpPost]
            [Authorize]
            [ProducesResponseType(typeof(BlogPostResponseDto), StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            public async Task<ActionResult<BlogPostResponseDto>> CreatePost([FromBody] CreateBlogPostDto dto)
            {
                var createdPost = await _blogPostService.CreatePostAsync(dto);
                return CreatedAtAction(nameof(GetPostById), new { id = createdPost.Id }, createdPost);
            }

            /// <summary>
            /// Update an existing blog post (Admin endpoint - Requires authentication)
            /// </summary>
            [HttpPut("{id}")]
            [Authorize]
            [ProducesResponseType(typeof(BlogPostResponseDto), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            public async Task<ActionResult<BlogPostResponseDto>> UpdatePost(int id, [FromBody] UpdateBlogPostDto dto)
            {
                if (id != dto.Id)
                    return BadRequest(new { message = "ID in URL does not match ID in body." });
                
                var updatedPost = await _blogPostService.UpdatePostAsync(dto);
                return Ok(updatedPost);
            }

            /// <summary>
            /// Delete a blog post (soft delete - Admin endpoint - Requires authentication)
            /// </summary>
            [HttpDelete("{id}")]
            [Authorize]
            [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            public async Task<IActionResult> DeletePost(int id)
            {
                var deleted = await _blogPostService.DeletePostAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Blog post with ID {id} not found." });
                return NoContent();
            }
        }
    }

