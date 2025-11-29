namespace PersonalBlog.API.DTOs.Projects
{
    public class CreateProjectDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? GitHubUrl { get; set; }
        public string? LiveUrl { get; set; }
        public int? DisplayOrder { get; set; }
        public List<int> SkillIds { get; set; } = new List<int>();
    }
}
