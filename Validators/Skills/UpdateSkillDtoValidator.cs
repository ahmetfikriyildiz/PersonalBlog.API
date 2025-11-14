using FluentValidation;
using PersonalBlog.API.DTOs.Skills;

namespace PersonalBlog.API.Validators.Skills
{
    public class UpdateSkillDtoValidator : AbstractValidator<UpdateSkillDto>
    {
        public UpdateSkillDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.Name)
                .Length(2, 80).When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("Skill name must be between 2 and 80 characters");

            RuleFor(x => x.Category)
                .MaximumLength(60).When(x => !string.IsNullOrEmpty(x.Category))
                .WithMessage("Category must not exceed 60 characters");

            RuleFor(x => x.Proficiency)
                .InclusiveBetween(1, 5).When(x => x.Proficiency.HasValue)
                .WithMessage("Proficiency must be between 1 and 5");
        }
    }
}

