using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Education;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;

namespace PersonalBlog.API.Repositories.Implementations
{
    public class EducationRepository : Repository<Education>, IEducationRepository
    {
        public EducationRepository(PersonalBlogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EducationResponseDto>> GetAllEducationsDtoAsync()
        {
            return await _context.Educations
                .Where(e => !e.IsDeleted)
                .OrderByDescending(e => e.StartDate)
                .Select(e => new EducationResponseDto
                {
                    Id = e.Id,
                    School = e.School,
                    Degree = e.Degree,
                    FieldOfStudy = e.FieldOfStudy,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<PagedResponse<EducationResponseDto>> GetAllEducationsDtoPagedAsync(PaginationFilter filter)
        {
            var query = _context.Educations.Where(e => !e.IsDeleted);
            var totalRecords = await query.CountAsync();

            var pagedData = await query
                .OrderByDescending(e => e.StartDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(e => new EducationResponseDto
                {
                    Id = e.Id,
                    School = e.School,
                    Degree = e.Degree,
                    FieldOfStudy = e.FieldOfStudy,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            return new PagedResponse<EducationResponseDto>(pagedData, filter.PageNumber, filter.PageSize, totalRecords);
        }

        public async Task<EducationResponseDto?> GetEducationByIdDtoAsync(int id)
        {
            return await _context.Educations
                .Where(e => e.Id == id && !e.IsDeleted)
                .Select(e => new EducationResponseDto
                {
                    Id = e.Id,
                    School = e.School,
                    Degree = e.Degree,
                    FieldOfStudy = e.FieldOfStudy,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    CreatedAt = e.CreatedAt
                })
                .FirstOrDefaultAsync();
        }
    }
}

