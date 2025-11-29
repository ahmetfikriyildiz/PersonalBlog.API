using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Education;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Repositories.Interfaces
{
    public interface IEducationRepository : IRepository<Education>
    {
        Task<IEnumerable<EducationResponseDto>> GetAllEducationsDtoAsync();
        Task<PagedResponse<EducationResponseDto>> GetAllEducationsDtoPagedAsync(PaginationFilter filter);
        Task<EducationResponseDto?> GetEducationByIdDtoAsync(int id);
    }
}

