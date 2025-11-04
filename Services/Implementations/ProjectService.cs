using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Projects;
using PersonalBlog.API.Mappings;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Interfaces;

namespace PersonalBlog.API.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly PersonalBlogDbContext _context; 

        public ProjectService(IProjectRepository projectRepository, PersonalBlogDbContext context)
        {
            _projectRepository = projectRepository;
            _context = context;
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            
            return await _projectRepository.GetAllProjectsWithSkillsAsync();
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int id)
        {
            
            return await _projectRepository.GetProjectByIdWithSkillsAsync(id);
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto)
        {
            
            var project = dto.ToEntity();

            
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

            
            await _projectRepository.CreateAsync(project);

            
            return await _projectRepository.GetProjectByIdWithSkillsAsync(project.Id)
                ?? throw new InvalidOperationException("Failed to retrieve created project.");
        }

        public async Task<ProjectResponseDto> UpdateProjectAsync(UpdateProjectDto dto)
        {
            
            var project = await _projectRepository.GetProjectWithSkillsAsync(dto.Id);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {dto.Id} not found.");

            
            dto.UpdateEntity(project);

            
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

            
            await _projectRepository.UpdateAsync(project);

            
            return await _projectRepository.GetProjectByIdWithSkillsAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Project with ID {dto.Id} not found after update.");
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            
            return await _projectRepository.DeleteAsync(id);
        }
    }
}