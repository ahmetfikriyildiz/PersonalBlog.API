using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Education;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PersonalBlog.API.Services.Implementations
{
    public class EducationService : IEducationService
    {
        private readonly IEducationRepository _educationRepository;
        private readonly PersonalBlogDbContext _context;

        public EducationService(IEducationRepository educationRepository, PersonalBlogDbContext context)
        {
            _educationRepository = educationRepository;
            _context = context;
        }

        public async Task<IEnumerable<EducationResponseDto>> GetAllEducationsAsync()
        {
            return await _educationRepository.GetAllEducationsDtoAsync();
        }

        public async Task<EducationResponseDto?> GetEducationByIdAsync(int id)
        {
            return await _educationRepository.GetEducationByIdDtoAsync(id);
        }

        public async Task<EducationResponseDto> CreateEducationAsync(CreateEducationDto dto)
        {
            // Ýlk User'ý al (þimdilik, sonra authentication ile deðiþecek)
            var user = await _context.Users.FirstOrDefaultAsync();
            if (user == null)
                throw new InvalidOperationException("No user found. Please create a user first.");

            var education = new Education
            {
                School = dto.School,
                Degree = dto.Degree,
                FieldOfStudy = dto.FieldOfStudy,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _educationRepository.CreateAsync(education);

            return await _educationRepository.GetEducationByIdDtoAsync(education.Id)
                ?? throw new InvalidOperationException("Failed to retrieve created education.");
        }

        public async Task<EducationResponseDto> UpdateEducationAsync(UpdateEducationDto dto)
        {
            var education = await _educationRepository.GetByIdAsync(dto.Id);

            if (education == null)
                throw new KeyNotFoundException($"Education with ID {dto.Id} not found.");

            if (dto.School != null)
                education.School = dto.School;

            if (dto.Degree != null)
                education.Degree = dto.Degree;

            if (dto.FieldOfStudy != null)
                education.FieldOfStudy = dto.FieldOfStudy;

            if (dto.StartDate.HasValue)
                education.StartDate = dto.StartDate.Value;

            if (dto.EndDate.HasValue)
                education.EndDate = dto.EndDate;

            education.UpdatedAt = DateTime.UtcNow;

            await _educationRepository.UpdateAsync(education);

            return await _educationRepository.GetEducationByIdDtoAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Education with ID {dto.Id} not found after update.");
        }

        public async Task<bool> DeleteEducationAsync(int id)
        {
            return await _educationRepository.DeleteAsync(id);
        }
    }
}

