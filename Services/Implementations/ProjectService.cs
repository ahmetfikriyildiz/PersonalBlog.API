using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Projects;
using PersonalBlog.API.Mappings;
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
            // Projection: Include yerine direkt Select ile DTO'ya map et
            var projects = await _context.Projects
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.DisplayOrder ?? 999)
                .ThenByDescending(p => p.CreatedAt)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Slug = p.Slug,
                    Description = p.Description,
                    GitHubUrl = p.GitHubUrl,
                    LiveUrl = p.LiveUrl,
                    DisplayOrder = p.DisplayOrder,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Skills = p.ProjectSkills
                        .Where(ps => !ps.Skill.IsDeleted)
                        .Select(ps => new SkillInfoDto
                        {
                            Id = ps.Skill.Id,
                            Name = ps.Skill.Name,
                            Category = ps.Skill.Category
                        })
                        .ToList()
                })
                .ToListAsync();

            return projects;
        }
        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
        {
            // Projection: Include yerine direkt Select ile DTO'ya map et
            var project = await _context.Projects
                .Where(p => p.Id == id && !p.IsDeleted)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Slug = p.Slug,
                    Description = p.Description,
                    GitHubUrl = p.GitHubUrl,
                    LiveUrl = p.LiveUrl,
                    DisplayOrder = p.DisplayOrder,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    Skills = p.ProjectSkills
                        .Where(ps => !ps.Skill.IsDeleted)
                        .Select(ps => new SkillInfoDto
                        {
                            Id = ps.Skill.Id,
                            Name = ps.Skill.Name,
                            Category = ps.Skill.Category
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return project;
        }
        public async Task<ProjectResponseDto> UpdateProjectAsync(UpdateProjectDto dto)
        {
            // Update için sadece entity'yi yükle, projection sonrası kullanılacak
            var project = await _context.Projects
                .Include(p => p.ProjectSkills) // Skills güncellemesi için gerekli
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

            // Reload yerine projection ile direkt DTO'ya map et
            return await GetProjectByIdAsync(dto.Id) 
                ?? throw new KeyNotFoundException($"Project with ID {dto.Id} not found after update.");
        }
        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto)
        {
            // 1. Yeni Project entity oluştur
            var project = new Project
            {
                Title = dto.Title,
                Slug = dto.Slug ?? ProjectMapping.GenerateSlug(dto.Title),
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

            // 4. Reload yerine projection ile direkt DTO'ya map et
            return await GetProjectByIdAsync(project.Id) 
                ?? throw new InvalidOperationException("Failed to retrieve created project.");
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
        // MapToResponseDto metodu artık kullanılmıyor - projection kullanıyoruz
        // Ancak gerekirse başka yerlerde kullanılabilir, o yüzden kaldırmıyoruz
        // private ProjectResponseDto MapToResponseDto(Project project) { ... }
        
    }
}
