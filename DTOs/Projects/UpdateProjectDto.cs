namespace PersonalBlog.API.DTOs.Projects
{
    public class UpdateProjectDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? GitHubUrl { get; set; }
        public string? LiveUrl { get; set; }
        public int? DisplayOrder { get; set; }
        public List<int>? SkillIds { get; set; }
    }
}
