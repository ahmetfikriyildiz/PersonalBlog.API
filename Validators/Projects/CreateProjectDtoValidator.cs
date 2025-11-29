using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Projects;

namespace PersonalBlog.API.Validators.Projects
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
    {
        private readonly PersonalBlogDbContext _context;

        public CreateProjectDtoValidator(PersonalBlogDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .Length(3, 150).WithMessage("Title must be between 3 and 150 characters");

            RuleFor(x => x.Slug)
                .MaximumLength(180).WithMessage("Slug must not exceed 180 characters")
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").When(x => !string.IsNullOrEmpty(x.Slug))
                .WithMessage("Slug must be lowercase alphanumeric with hyphens only");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

            RuleFor(x => x.GitHubUrl)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.GitHubUrl))
                .WithMessage("Invalid GitHub URL format")
                .MaximumLength(400).WithMessage("GitHub URL must not exceed 400 characters");

            RuleFor(x => x.LiveUrl)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.LiveUrl))
                .WithMessage("Invalid Live URL format")
                .MaximumLength(400).WithMessage("Live URL must not exceed 400 characters");

            RuleFor(x => x.DisplayOrder)
                .InclusiveBetween(0, 1000).When(x => x.DisplayOrder.HasValue)
                .WithMessage("DisplayOrder must be between 0 and 1000");

            RuleFor(x => x.SkillIds)
                .NotNull().WithMessage("SkillIds cannot be null")
                .MustAsync(async (skillIds, cancellation) =>
                {
                    if (skillIds == null || !skillIds.Any())
                        return true;

                    // Tüm SkillIds'lerin geçerli skill'ler olduğunu kontrol et
                    var validSkillIds = await _context.Skills
                        .Where(s => skillIds.Contains(s.Id) && !s.IsDeleted)
                        .Select(s => s.Id)
                        .ToListAsync(cancellation);

                    return validSkillIds.Count == skillIds.Count;
                })
                .When(x => x.SkillIds != null && x.SkillIds.Any())
                .WithMessage("One or more skill IDs are invalid or do not exist.");
        }

        private bool BeValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var result) &&
                   (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }
    }
}

