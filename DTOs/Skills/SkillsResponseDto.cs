namespace PersonalBlog.API.DTOs.Skills
{
    public class SkillsResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Proficiency { get; set; } // 1-5
        public DateTime CreatedAt { get; set; }
    }
}
