using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Experience;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Repositories.Interfaces
{
    public interface IExperienceRepository : IRepository<Experience>
    {
        Task<IEnumerable<ExperienceResponseDto>> GetAllExperiencesDtoAsync();
        Task<PagedResponse<ExperienceResponseDto>> GetAllExperiencesDtoPagedAsync(PaginationFilter filter);
        Task<ExperienceResponseDto?> GetExperienceByIdDtoAsync(int id);
    }
}

