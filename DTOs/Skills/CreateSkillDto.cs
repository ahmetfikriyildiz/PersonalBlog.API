using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.Skills
{
    public class CreateSkillDto
    {
        [Required(ErrorMessage = "Skill name is required")]
        [StringLength(80, MinimumLength = 2, ErrorMessage = "Skill name must be between 2 and 80 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(60)]
        public string? Category { get; set; } // Backend, Frontend, Database, etc.

        [Range(1, 5, ErrorMessage = "Proficiency must be between 1 and 5")]
        public int Proficiency { get; set; } = 1;
    }
}
