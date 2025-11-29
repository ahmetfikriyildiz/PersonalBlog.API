namespace PersonalBlog.API.DTOs.Skills
{
    public class UpdateSkillDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public int? Proficiency { get; set; }
    }
}
