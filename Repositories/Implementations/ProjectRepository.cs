using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Projects;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;

namespace PersonalBlog.API.Repositories.Implementations
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(PersonalBlogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsWithSkillsAsync()
        {
            // Projection ile direkt DTO'ya map et
            return await _context.Projects
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
        }

        public async Task<ProjectResponseDto?> GetProjectByIdWithSkillsAsync(int id)
        {
            // Projection ile direkt DTO'ya map et
            return await _context.Projects
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
        }

        public async Task<Project?> GetProjectWithSkillsAsync(int id)
        {
            // Update için entity'yi Skills ile birlikte getir
            return await _context.Projects
                .Include(p => p.ProjectSkills)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }
    }
}