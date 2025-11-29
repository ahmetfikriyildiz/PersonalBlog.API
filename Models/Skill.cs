namespace PersonalBlog.API.Models
{
    public class Skill : BaseEntity
    {
        public string? Name { get; set; } // Unique
        public string? Category { get; set; } // e.g., Backend, Frontend, DB
        public int Proficiency { get; set; } // 1-5

        public ICollection<ProjectSkill> ProjectSkills { get; set; } = new List<ProjectSkill>();
    }
}
