using PersonalBlog.API.DTOs.Projects;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Mappings
{
    public static class ProjectMapping
    {
        // DTO → Entity Mapping
        public static Project ToEntity(this CreateProjectDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Project
            {
                Title = dto.Title,
                Slug = dto.Slug ?? GenerateSlug(dto.Title),
                Description = dto.Description,
                GitHubUrl = dto.GitHubUrl,
                LiveUrl = dto.LiveUrl,
                DisplayOrder = dto.DisplayOrder,
                CreatedAt = DateTime.UtcNow
            };
        }

        // DTO → Entity Mapping (Update için)
        public static void UpdateEntity(this UpdateProjectDto dto, Project entity)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (!string.IsNullOrWhiteSpace(dto.Title))
                entity.Title = dto.Title;

            if (dto.Slug != null)
                entity.Slug = dto.Slug;

            if (dto.Description != null)
                entity.Description = dto.Description;

            if (dto.GitHubUrl != null)
                entity.GitHubUrl = dto.GitHubUrl;

            if (dto.LiveUrl != null)
                entity.LiveUrl = dto.LiveUrl;

            if (dto.DisplayOrder.HasValue)
                entity.DisplayOrder = dto.DisplayOrder;

            entity.UpdatedAt = DateTime.UtcNow;
        }

        // Entity → DTO Mapping
        public static ProjectResponseDto ToResponseDto(this Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            return new ProjectResponseDto
            {
                Id = project.Id,
                Title = project.Title,
                Slug = project.Slug,
                Description = project.Description,
                GitHubUrl = project.GitHubUrl,
                LiveUrl = project.LiveUrl,
                DisplayOrder = project.DisplayOrder,
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                Skills = project.ProjectSkills
                    .Select(ps => new SkillInfoDto
                    {
                        Id = ps.Skill.Id,
                        Name = ps.Skill.Name,
                        Category = ps.Skill.Category
                    })
                    .ToList()
            };
        }

        // Collection Mapping
        public static IEnumerable<ProjectResponseDto> ToResponseDtoList(
            this IEnumerable<Project> projects)
        {
            if (projects == null)
                return Enumerable.Empty<ProjectResponseDto>();

            return projects.Select(p => p.ToResponseDto());
        }

        // Helper: Slug generation
        public static string GenerateSlug(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;

            return title.ToLower()
                .Trim()
                .Replace(" ", "-")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ç", "c")
                .Replace("&", "and")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("?", "")
                .Replace("!", "");
        }
    }
}