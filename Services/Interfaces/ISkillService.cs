using PersonalBlog.API.DTOs.Skills;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface ISkillService
    {
        Task<IEnumerable<SkillsResponseDto>> GetAllSkillsAsync();
        Task<SkillsResponseDto?> GetSkillByIdAsync(int id);
        Task<SkillsResponseDto> CreateSkillAsync(CreateSkillDto dto);
        Task<SkillsResponseDto> UpdateSkillAsync(UpdateSkillDto dto);
        Task<bool> DeleteSkillAsync(int id);
    }
}