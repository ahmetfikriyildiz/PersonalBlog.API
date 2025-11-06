using PersonalBlog.API.DTOs.Experience;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Repositories.Interfaces
{
    public interface IExperienceRepository : IRepository<Experience>
    {
        Task<IEnumerable<ExperienceResponseDto>> GetAllExperiencesDtoAsync();
        Task<ExperienceResponseDto?> GetExperienceByIdDtoAsync(int id);
    }
}

