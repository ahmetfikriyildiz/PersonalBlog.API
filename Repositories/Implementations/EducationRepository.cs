using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
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

