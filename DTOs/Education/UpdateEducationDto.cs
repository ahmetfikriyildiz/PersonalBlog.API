using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.Education
{
    public class UpdateEducationDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(200)]
        public string? School { get; set; }

        [StringLength(100)]
        public string? Degree { get; set; }

        [StringLength(100)]
        public string? FieldOfStudy { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}

