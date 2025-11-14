using FluentValidation;
using PersonalBlog.API.DTOs.Education;

namespace PersonalBlog.API.Validators.Education
{
    public class CreateEducationDtoValidator : AbstractValidator<CreateEducationDto>
    {
        public CreateEducationDtoValidator()
        {
            RuleFor(x => x.School)
                .NotEmpty().WithMessage("School is required")
                .MaximumLength(200).WithMessage("School must not exceed 200 characters");

            RuleFor(x => x.Degree)
                .NotEmpty().WithMessage("Degree is required")
                .MaximumLength(100).WithMessage("Degree must not exceed 100 characters");

            RuleFor(x => x.FieldOfStudy)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.FieldOfStudy))
                .WithMessage("Field of study must not exceed 100 characters");

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

