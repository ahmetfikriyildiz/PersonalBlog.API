using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Experience;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PersonalBlog.API.Services.Implementations
{
    public class ExperienceService : IExperienceService
    {
        private readonly IExperienceRepository _experienceRepository;
        private readonly PersonalBlogDbContext _context;

        public ExperienceService(IExperienceRepository experienceRepository, PersonalBlogDbContext context)
        {
            _experienceRepository = experienceRepository;
            _context = context;
        }

        public async Task<IEnumerable<ExperienceResponseDto>> GetAllExperiencesAsync()
        {
            return await _experienceRepository.GetAllExperiencesDtoAsync();
        }

        public async Task<PagedResponse<ExperienceResponseDto>> GetAllExperiencesPagedAsync(PaginationFilter filter)
        {
            return await _experienceRepository.GetAllExperiencesDtoPagedAsync(filter);
        }

        public async Task<ExperienceResponseDto?> GetExperienceByIdAsync(int id)
        {
            return await _experienceRepository.GetExperienceByIdDtoAsync(id);
        }

        public async Task<ExperienceResponseDto> CreateExperienceAsync(CreateExperienceDto dto)
        {
            // İlk User'ı al (şimdilik, sonra authentication ile değişecek)
            var user = await _context.Users.FirstOrDefaultAsync();
            if (user == null)
                throw new InvalidOperationException("No user found. Please create a user first.");

            var experience = new Experience
            {
                Company = dto.Company,
                Role = dto.Role,
                Location = dto.Location,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _experienceRepository.CreateAsync(experience);

            return await _experienceRepository.GetExperienceByIdDtoAsync(experience.Id)
                ?? throw new InvalidOperationException("Failed to retrieve created experience.");
        }

        public async Task<ExperienceResponseDto> UpdateExperienceAsync(UpdateExperienceDto dto)
        {
            var experience = await _experienceRepository.GetByIdAsync(dto.Id);
            
            if (experience == null)
                throw new KeyNotFoundException($"Experience with ID {dto.Id} not found.");

            if (dto.Company != null)
                experience.Company = dto.Company;

            if (dto.Role != null)
                experience.Role = dto.Role;

            if (dto.Location != null)
                experience.Location = dto.Location;

            if (dto.Description != null)
                experience.Description = dto.Description;

            if (dto.StartDate.HasValue)
                experience.StartDate = dto.StartDate.Value;

            if (dto.EndDate.HasValue)
                experience.EndDate = dto.EndDate;

            experience.UpdatedAt = DateTime.UtcNow;

            await _experienceRepository.UpdateAsync(experience);

            return await _experienceRepository.GetExperienceByIdDtoAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Experience with ID {dto.Id} not found after update.");
        }

        public async Task<bool> DeleteExperienceAsync(int id)
        {
            return await _experienceRepository.DeleteAsync(id);
        }
    }
}

