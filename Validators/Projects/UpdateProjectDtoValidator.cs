using FluentValidation;
using PersonalBlog.API.DTOs.Projects;

namespace PersonalBlog.API.Validators.Projects
{
    public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.Title)
                .Length(3, 150).When(x => !string.IsNullOrEmpty(x.Title))
                .WithMessage("Title must be between 3 and 150 characters");

            RuleFor(x => x.Slug)
                .MaximumLength(180).When(x => !string.IsNullOrEmpty(x.Slug))
                .WithMessage("Slug must not exceed 180 characters")
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").When(x => !string.IsNullOrEmpty(x.Slug))
                .WithMessage("Slug must be lowercase alphanumeric with hyphens only");

            RuleFor(x => x.Description)
                .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description must not exceed 2000 characters");

            RuleFor(x => x.GitHubUrl)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.GitHubUrl))
                .WithMessage("Invalid GitHub URL format")
                .MaximumLength(400).When(x => !string.IsNullOrEmpty(x.GitHubUrl))
                .WithMessage("GitHub URL must not exceed 400 characters");

            RuleFor(x => x.LiveUrl)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.LiveUrl))
                .WithMessage("Invalid Live URL format")
                .MaximumLength(400).When(x => !string.IsNullOrEmpty(x.LiveUrl))
                .WithMessage("Live URL must not exceed 400 characters");

            RuleFor(x => x.DisplayOrder)
                .InclusiveBetween(0, 1000).When(x => x.DisplayOrder.HasValue)
                .WithMessage("DisplayOrder must be between 0 and 1000");
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

