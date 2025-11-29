using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Education;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IEducationService
    {
        Task<IEnumerable<EducationResponseDto>> GetAllEducationsAsync();
        Task<PagedResponse<EducationResponseDto>> GetAllEducationsPagedAsync(PaginationFilter filter);
        Task<EducationResponseDto?> GetEducationByIdAsync(int id);
        Task<EducationResponseDto> CreateEducationAsync(CreateEducationDto dto);
        Task<EducationResponseDto> UpdateEducationAsync(UpdateEducationDto dto);
        Task<bool> DeleteEducationAsync(int id);
    }
}

