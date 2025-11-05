using PersonalBlog.API.DTOs.Skills;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Repositories.Interfaces
{
    public interface ISkillRepository : IRepository<Skill>
    {
        Task<IEnumerable<SkillsResponseDto>> GetAllSkillsDtoAsync();
        Task<SkillsResponseDto?> GetSkillByIdDtoAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}