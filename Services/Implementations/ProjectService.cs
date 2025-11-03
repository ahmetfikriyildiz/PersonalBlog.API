using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Projects;
using PersonalBlog.API.Exceptions;
using PersonalBlog.API.Mappings;
using PersonalBlog.API.Models;
using PersonalBlog.API.Services.Interfaces;

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

        return projects.ToResponseDtoList();
    }

    public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
    {
        var project = await _context.Projects
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.Skill)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (project == null)
            return null;

        return project.ToResponseDto();
    }

    public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto)
    {
        // Validation
        var existingSlug = await _context.Projects
            .AnyAsync(p => p.Slug == (dto.Slug ?? ProjectMapping.GenerateSlug(dto.Title)) && !p.IsDeleted);

        if (existingSlug)
            throw new ConflictException("A project with this slug already exists.");

        var project = dto.ToEntity();

        // Skills ilişkilerini ekle
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

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Reload ile Skills dahil et
        await _context.Entry(project)
            .Collection(p => p.ProjectSkills)
            .Query()
            .Include(ps => ps.Skill)
            .LoadAsync();

        return project.ToResponseDto();
    }

    public async Task<ProjectResponseDto> UpdateProjectAsync(UpdateProjectDto dto)
    {
        var project = await _context.Projects
            .Include(p => p.ProjectSkills)
            .FirstOrDefaultAsync(p => p.Id == dto.Id && !p.IsDeleted);

        if (project == null)
            throw new NotFoundException("Project", dto.Id);

        dto.UpdateEntity(project);

        // Skills güncellemesi
        if (dto.SkillIds != null)
        {
            project.ProjectSkills.Clear();

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

        await _context.SaveChangesAsync();

        // Reload
        await _context.Entry(project)
            .Collection(p => p.ProjectSkills)
            .Query()
            .Include(ps => ps.Skill)
            .LoadAsync();

        return project.ToResponseDto();
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (project == null)
            throw new NotFoundException("Project", id);

        project.IsDeleted = true;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}