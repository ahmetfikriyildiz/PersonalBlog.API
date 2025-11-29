using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Projects;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Repositories.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllProjectsWithSkillsAsync();
        Task<PagedResponse<ProjectResponseDto>> GetAllProjectsWithSkillsPagedAsync(PaginationFilter filter);
        Task<ProjectResponseDto?> GetProjectByIdWithSkillsAsync(int id);
        Task<Project?> GetProjectWithSkillsAsync(int id);
    }
}
