using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Projects;
using PersonalBlog.API.Models;
using PersonalBlog.API.Services.Interfaces;

namespace PersonalBlog.API.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly PersonalBlogDbContext _context;

        public ProjectService(PersonalBlogDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
        var projects = await _context.Projects
        .Include(p => p.ProjectSkills)
            .ThenInclude(ps => ps.Skill)
        .Where(p => !p.IsDeleted)
        .OrderBy(p => p.DisplayOrder ?? 999)
        .ThenByDescending(p => p.CreatedAt)
        .ToListAsync();

            return projects.Select(MapToResponseDto);
        }
        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects
        .Include(p => p.ProjectSkills)
            .ThenInclude(ps => ps.Skill)
        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (project == null)
                return null;

            return MapToResponseDto(project);
        }
        public async Task<ProjectResponseDto> UpdateProjectAsync(UpdateProjectDto dto)
        {
            var project = await _context.Projects
         .Include(p => p.ProjectSkills)
         .FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsDeleted);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {dto.Id} not found.");

            // Sadece gönderilen alanları güncelle (partial update)
            if (!string.IsNullOrWhiteSpace(dto.Title))
                project.Title = dto.Title;

            if (dto.Slug != null)
                project.Slug = dto.Slug;

            if (dto.Description != null)
                project.Description = dto.Description;

            if (dto.GitHubUrl != null)
                project.GitHubUrl = dto.GitHubUrl;

            if (dto.LiveUrl != null)
                project.LiveUrl = dto.LiveUrl;

            if (dto.DisplayOrder.HasValue)
                project.DisplayOrder = dto.DisplayOrder;

            // Skills güncellemesi
            if (dto.SkillIds != null)
            {
                // Mevcut ilişkileri temizle
                project.ProjectSkills.Clear();

                // Yeni skills ekle
                var skills = await _context.Skills
                    .Where(s => dto.SkillIds.Contains(s.Id) && !s.IsDeleted)
                    .ToListAsync();

                foreach (var skill in skills)
                {
                    project.ProjectSkills.Add(new ProjectSkill
                    {
                        Project = project,
                        Skill = skill
                    });
                }
            }

            project.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Reload
            await _context.Entry(project)
                .Collection(p => p.ProjectSkills)
                .Query()
                .Include(ps => ps.Skill)
                .LoadAsync();

            return MapToResponseDto(project);
        }
        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto)
        {
            // 1. Yeni Project entity oluştur
            var project = new Project
            {
                Title = dto.Title,
                Slug = dto.Slug ?? GenerateSlug(dto.Title),
                Description = dto.Description,
                GitHubUrl = dto.GitHubUrl,
                LiveUrl = dto.LiveUrl,
                DisplayOrder = dto.DisplayOrder,
                CreatedAt = DateTime.UtcNow
            };

            // 2. Skills ilişkilerini ekle
            if (dto.SkillIds != null && dto.SkillIds.Any())
            {
                var skills = await _context.Skills
                    .Where(s => dto.SkillIds.Contains(s.Id) && !s.IsDeleted)
                    .ToListAsync();

                foreach (var skill in skills)
                {
                    project.ProjectSkills.Add(new ProjectSkill
                    {
                        Project = project,
                        Skill = skill
                    });
                }
            }

            // 3. Veritabanına kaydet
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // 4. Reload ile Skills'i dahil et ve Response DTO döndür
            await _context.Entry(project)
                .Collection(p => p.ProjectSkills)
                .Query()
                .Include(ps => ps.Skill)
                .LoadAsync();

            return MapToResponseDto(project);
        }
        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects
         .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (project == null)
                return false;

            // Soft delete
            project.IsDeleted = true;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        private ProjectResponseDto MapToResponseDto(Project project)
        {
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
        private string GenerateSlug(string title)
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
