using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.Projects
{
    public class UpdateProjectDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(150, MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(180)]
        public string? Slug { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [Url]
        [StringLength(400)]
        public string? GitHubUrl { get; set; }

        [Url]
        [StringLength(400)]
        public string? LiveUrl { get; set; }

        [Range(0, 1000)]
        public int? DisplayOrder { get; set; }

        // Skills güncellenebilir
        public List<int>? SkillIds { get; set; }
    }
}
