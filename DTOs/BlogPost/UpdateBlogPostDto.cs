using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.BlogPost
{
    public class UpdateBlogPostDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(250)]
        public string? Slug { get; set; }

        public string? Content { get; set; }

        public bool? IsPublished { get; set; }
    }
}

