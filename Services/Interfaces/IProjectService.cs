using PersonalBlog.API.DTOs.Projects;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync();
        Task<ProjectResponseDto?> GetProjectByIdAsync(int id);
        Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto dto);
        Task<ProjectResponseDto> UpdateProjectAsync(UpdateProjectDto dto);
        Task<bool> DeleteProjectAsync(int id);
    }
}
