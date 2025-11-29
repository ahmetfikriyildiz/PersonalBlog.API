using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Experience;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IExperienceService
    {
        Task<IEnumerable<ExperienceResponseDto>> GetAllExperiencesAsync();
        Task<PagedResponse<ExperienceResponseDto>> GetAllExperiencesPagedAsync(PaginationFilter filter);
        Task<ExperienceResponseDto?> GetExperienceByIdAsync(int id);
        Task<ExperienceResponseDto> CreateExperienceAsync(CreateExperienceDto dto);
        Task<ExperienceResponseDto> UpdateExperienceAsync(UpdateExperienceDto dto);
        Task<bool> DeleteExperienceAsync(int id);
    }
}

