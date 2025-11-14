using FluentValidation;
using PersonalBlog.API.DTOs.Experience;

namespace PersonalBlog.API.Validators.Experience
{
    public class CreateExperienceDtoValidator : AbstractValidator<CreateExperienceDto>
    {
        public CreateExperienceDtoValidator()
        {
            RuleFor(x => x.Company)
                .NotEmpty().WithMessage("Company is required")
                .MaximumLength(200).WithMessage("Company must not exceed 200 characters");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .MaximumLength(100).WithMessage("Role must not exceed 100 characters");

            RuleFor(x => x.Location)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Location))
                .WithMessage("Location must not exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Start date cannot be in the future");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).When(x => x.EndDate.HasValue)
                .WithMessage("End date must be after start date")
                .LessThanOrEqualTo(DateTime.Today).When(x => x.EndDate.HasValue)
                .WithMessage("End date cannot be in the future");
        }
    }
}

