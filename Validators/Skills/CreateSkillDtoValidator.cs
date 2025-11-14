using FluentValidation;
using PersonalBlog.API.DTOs.Skills;

namespace PersonalBlog.API.Validators.Skills
{
    public class CreateSkillDtoValidator : AbstractValidator<CreateSkillDto>
    {
        public CreateSkillDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Skill name is required")
                .Length(2, 80).WithMessage("Skill name must be between 2 and 80 characters");

            RuleFor(x => x.Category)
                .MaximumLength(60).When(x => !string.IsNullOrEmpty(x.Category))
                .WithMessage("Category must not exceed 60 characters");

            RuleFor(x => x.Proficiency)
                .InclusiveBetween(1, 5).WithMessage("Proficiency must be between 1 and 5");
        }
    }
}

