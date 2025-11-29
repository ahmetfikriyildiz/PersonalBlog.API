namespace PersonalBlog.API.DTOs.Experience
{
    public class UpdateExperienceDto
    {
        public int Id { get; set; }
        public string? Company { get; set; }
        public string? Role { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

