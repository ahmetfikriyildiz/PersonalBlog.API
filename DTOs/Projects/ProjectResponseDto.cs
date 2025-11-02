namespace PersonalBlog.API.DTOs.Projects
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? GitHubUrl { get; set; }
        public string? LiveUrl { get; set; }
        public int? DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Skills detaylı bilgi ile
        public List<SkillInfoDto> Skills { get; set; } = new List<SkillInfoDto>();
    }

    public class SkillInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
    }
}
