using FluentValidation;
using PersonalBlog.API.DTOs.Education;

namespace PersonalBlog.API.Validators.Education
{
    public class UpdateEducationDtoValidator : AbstractValidator<UpdateEducationDto>
    {
        public UpdateEducationDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.School)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.School))
                .WithMessage("School must not exceed 200 characters");

            RuleFor(x => x.Degree)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Degree))
                .WithMessage("Degree must not exceed 100 characters");

            RuleFor(x => x.FieldOfStudy)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.FieldOfStudy))
                .WithMessage("Field of study must not exceed 100 characters");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(DateTime.Today).When(x => x.StartDate.HasValue)
                .WithMessage("Start date cannot be in the future");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate!.Value).When(x => x.EndDate.HasValue && x.StartDate.HasValue)
                .WithMessage("End date must be after start date")
                .LessThanOrEqualTo(DateTime.Today).When(x => x.EndDate.HasValue)
                .WithMessage("End date cannot be in the future");
        }
    }
}

