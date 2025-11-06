using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.Experience
{
    public class UpdateExperienceDto
    {
        [Required]
        public int Id { get; set; }

        [StringLength(200)]
        public string? Company { get; set; }

        [StringLength(100)]
        public string? Role { get; set; }

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}

