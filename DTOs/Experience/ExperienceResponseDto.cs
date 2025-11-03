namespace PersonalBlog.API.DTOs.Experience
{
    public class ExperienceResponseDto
    {
        public int Id { get; set; }
        public string Company { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
