using FluentValidation;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Skills;
using PersonalBlog.API.Validators.Custom;

namespace PersonalBlog.API.Validators.Skills
{
    public class UpdateSkillDtoValidator : AbstractValidator<UpdateSkillDto>
    {
        private readonly PersonalBlogDbContext _context;

        public UpdateSkillDtoValidator(PersonalBlogDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.Name)
                .Length(2, 80).When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage("Skill name must be between 2 and 80 characters")
                .MustBeUniqueSkillName(_context, x => x.Id)
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Category)
                .MaximumLength(60).When(x => !string.IsNullOrEmpty(x.Category))
                .WithMessage("Category must not exceed 60 characters");

            RuleFor(x => x.Proficiency)
                .InclusiveBetween(1, 5).When(x => x.Proficiency.HasValue)
                .WithMessage("Proficiency must be between 1 and 5");
        }
    }
}

