namespace PersonalBlog.API.DTOs.Skills
{
    public class CreateSkillDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public int Proficiency { get; set; } = 1;
    }
}
