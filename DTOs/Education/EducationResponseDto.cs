namespace PersonalBlog.API.DTOs.Education
{
    public class EducationResponseDto
    {
        public int Id { get; set; }
        public string School { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string? FieldOfStudy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
