using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Skills;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;

namespace PersonalBlog.API.Repositories.Implementations
{
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        public SkillRepository(PersonalBlogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SkillsResponseDto>> GetAllSkillsDtoAsync()
        {
            // Projection ile direkt DTO'ya map et
            return await _context.Skills
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.Category)
                .ThenBy(s => s.Name)
                .Select(s => new SkillsResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category,
                    Proficiency = s.Proficiency,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<SkillsResponseDto?> GetSkillByIdDtoAsync(int id)
        {
            // Projection ile direkt DTO'ya map et
            return await _context.Skills
                .Where(s => s.Id == id && !s.IsDeleted)
                .Select(s => new SkillsResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Category = s.Category,
                    Proficiency = s.Proficiency,
                    CreatedAt = s.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Skills
                .AnyAsync(s => s.Name.ToLower() == name.ToLower() && !s.IsDeleted);
        }
    }
}