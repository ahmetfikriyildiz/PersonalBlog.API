using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Skills;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Repositories.Interfaces
{
    public interface ISkillRepository : IRepository<Skill>
    {
        Task<IEnumerable<SkillsResponseDto>> GetAllSkillsDtoAsync();
        Task<PagedResponse<SkillsResponseDto>> GetAllSkillsDtoPagedAsync(PaginationFilter filter);
        Task<SkillsResponseDto?> GetSkillByIdDtoAsync(int id);
    }
}