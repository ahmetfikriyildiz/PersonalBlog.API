using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.Projects
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 150 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(180)]
        public string? Slug { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [Url(ErrorMessage = "Invalid GitHub URL format")]
        [StringLength(400)]
        public string? GitHubUrl { get; set; }

        [Url(ErrorMessage = "Invalid Live URL format")]
        [StringLength(400)]
        public string? LiveUrl { get; set; }

        [Range(0, 1000, ErrorMessage = "DisplayOrder must be between 0 and 1000")]
        public int? DisplayOrder { get; set; }

        // Skills için ID listesi
        public List<int> SkillIds { get; set; } = new List<int>();
    }
}
