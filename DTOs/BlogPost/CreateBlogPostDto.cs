using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.BlogPost
{
    public class CreateBlogPostDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required")]
        [StringLength(250)]
        public string Slug { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;

        public bool IsPublished { get; set; } = false;
    }
}
