using FluentValidation;
using PersonalBlog.API.DTOs.Experience;

namespace PersonalBlog.API.Validators.Experience
{
    public class UpdateExperienceDtoValidator : AbstractValidator<UpdateExperienceDto>
    {
        public UpdateExperienceDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.Company)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Company))
                .WithMessage("Company must not exceed 200 characters");

            RuleFor(x => x.Role)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Role))
                .WithMessage("Role must not exceed 100 characters");

            RuleFor(x => x.Location)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Location))
                .WithMessage("Location must not exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description))
                .WithMessage("Description must not exceed 1000 characters");

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

