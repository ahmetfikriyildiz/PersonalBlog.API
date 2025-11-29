using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Experience;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;

namespace PersonalBlog.API.Repositories.Implementations
{
    public class ExperienceRepository : Repository<Experience>, IExperienceRepository
    {
        public ExperienceRepository(PersonalBlogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ExperienceResponseDto>> GetAllExperiencesDtoAsync()
        {
            return await _context.Experiences
                .Where(e => !e.IsDeleted)
                .OrderByDescending(e => e.StartDate)
                .Select(e => new ExperienceResponseDto
                {
                    Id = e.Id,
                    Company = e.Company,
                    Role = e.Role,
                    Location = e.Location,
                    Description = e.Description,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<PagedResponse<ExperienceResponseDto>> GetAllExperiencesDtoPagedAsync(PaginationFilter filter)
        {
            var query = _context.Experiences.Where(e => !e.IsDeleted);
            var totalRecords = await query.CountAsync();

            var pagedData = await query
                .OrderByDescending(e => e.StartDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(e => new ExperienceResponseDto
                {
                    Id = e.Id,
                    Company = e.Company,
                    Role = e.Role,
                    Location = e.Location,
                    Description = e.Description,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            return new PagedResponse<ExperienceResponseDto>(pagedData, filter.PageNumber, filter.PageSize, totalRecords);
        }

        public async Task<ExperienceResponseDto?> GetExperienceByIdDtoAsync(int id)
        {
            return await _context.Experiences
                .Where(e => e.Id == id && !e.IsDeleted)
                .Select(e => new ExperienceResponseDto
                {
                    Id = e.Id,
                    Company = e.Company,
                    Role = e.Role,
                    Location = e.Location,
                    Description = e.Description,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    CreatedAt = e.CreatedAt
                })
                .FirstOrDefaultAsync();
        }
    }
}

