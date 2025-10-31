namespace PersonalBlog.API.Models
{
    public class Project : BaseEntity
    {
        public string Title { get; set; }
        public string? Slug { get; set; } // URL dostu
        public string? Description { get; set; }
        public string? GitHubUrl { get; set; }
        public string? LiveUrl { get; set; }
        public int? DisplayOrder { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public ICollection<ProjectSkill> ProjectSkills { get; set; } = new List<ProjectSkill>();
    }
}
