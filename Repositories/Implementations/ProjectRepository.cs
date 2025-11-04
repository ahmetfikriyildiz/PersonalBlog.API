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
        public Task<IEnumerable<ProjectResponseDto>> GetAllProjectsWithSkillsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProjectResponseDto?> GetProjectByIdWithSkillsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Project> GetProjectWithSkillsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
