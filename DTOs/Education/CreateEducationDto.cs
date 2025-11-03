using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.Education
{
    public class CreateEducationDto
    {
        [Required]
        [StringLength(200)]
        public string School { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Degree { get; set; } = string.Empty;

        [StringLength(100)]
        public string? FieldOfStudy { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
