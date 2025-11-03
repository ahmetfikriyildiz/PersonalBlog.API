using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.Experience
{
    public class CreateExperienceDto
    {
        [Required]
        [StringLength(200)]
        public string Company { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Role { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
