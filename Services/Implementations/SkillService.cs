using PersonalBlog.API.DTOs.Skills;
using PersonalBlog.API.Mappings;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Interfaces;

namespace PersonalBlog.API.Services.Implementations
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<IEnumerable<SkillsResponseDto>> GetAllSkillsAsync()
        {
            return await _skillRepository.GetAllSkillsDtoAsync();
        }

        public async Task<SkillsResponseDto?> GetSkillByIdAsync(int id)
        {
            return await _skillRepository.GetSkillByIdDtoAsync(id);
        }

        public async Task<SkillsResponseDto> CreateSkillAsync(CreateSkillDto dto)
        {
            // Duplicate name kontrolü
            if (await _skillRepository.ExistsByNameAsync(dto.Name))
                throw new InvalidOperationException($"A skill with name '{dto.Name}' already exists.");

            // DTO'dan Entity'ye map et
            var skill = new Skill
            {
                Name = dto.Name,
                Category = dto.Category,
                Proficiency = dto.Proficiency,
                CreatedAt = DateTime.UtcNow
            };

            await _skillRepository.CreateAsync(skill);

            // Repository'den DTO olarak geri döndür
            return await _skillRepository.GetSkillByIdDtoAsync(skill.Id)
                ?? throw new InvalidOperationException("Failed to retrieve created skill.");
        }

        public async Task<SkillsResponseDto> UpdateSkillAsync(UpdateSkillDto dto)
        {
            var skill = await _skillRepository.GetByIdAsync(dto.Id);

            if (skill == null)
                throw new KeyNotFoundException($"Skill with ID {dto.Id} not found.");

            // Name değişiyorsa duplicate kontrolü
            if (dto.Name != null && dto.Name != skill.Name)
            {
                if (await _skillRepository.ExistsByNameAsync(dto.Name))
                    throw new InvalidOperationException($"A skill with name '{dto.Name}' already exists.");
            }

            // Update
            if (dto.Name != null)
                skill.Name = dto.Name;

            if (dto.Category != null)
                skill.Category = dto.Category;

            if (dto.Proficiency.HasValue)
                skill.Proficiency = dto.Proficiency.Value;

            skill.UpdatedAt = DateTime.UtcNow;

            await _skillRepository.UpdateAsync(skill);

            // Repository'den DTO olarak geri döndür
            return await _skillRepository.GetSkillByIdDtoAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Skill with ID {dto.Id} not found after update.");
        }

        public async Task<bool> DeleteSkillAsync(int id)
        {
            return await _skillRepository.DeleteAsync(id);
        }
    }
}