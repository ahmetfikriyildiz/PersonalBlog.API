using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.Skills
{
    public class UpdateSkillDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(80, MinimumLength = 2)]
        public string? Name { get; set; }

        [StringLength(60)]
        public string? Category { get; set; }

        [Range(1, 5)]
        public int? Proficiency { get; set; }
    }
}
