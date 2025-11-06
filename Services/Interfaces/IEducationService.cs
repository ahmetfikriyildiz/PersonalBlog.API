using PersonalBlog.API.DTOs.Education;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IEducationService
    {
        Task<IEnumerable<EducationResponseDto>> GetAllEducationsAsync();
        Task<EducationResponseDto?> GetEducationByIdAsync(int id);
        Task<EducationResponseDto> CreateEducationAsync(CreateEducationDto dto);
        Task<EducationResponseDto> UpdateEducationAsync(UpdateEducationDto dto);
        Task<bool> DeleteEducationAsync(int id);
    }
}

