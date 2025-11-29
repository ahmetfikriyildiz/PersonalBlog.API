using FluentValidation;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.BlogPost;
using PersonalBlog.API.Validators.Custom;

namespace PersonalBlog.API.Validators.BlogPost
{
    public class UpdateBlogPostDtoValidator : AbstractValidator<UpdateBlogPostDto>
    {
        private readonly PersonalBlogDbContext _context;

        public UpdateBlogPostDtoValidator(PersonalBlogDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.Title)
                .Length(3, 200).When(x => !string.IsNullOrEmpty(x.Title))
                .WithMessage("Title must be between 3 and 200 characters");

            RuleFor(x => x.Slug)
                .MaximumLength(250).When(x => !string.IsNullOrEmpty(x.Slug))
                .WithMessage("Slug must not exceed 250 characters")
                .MustBeValidSlugFormat()
                .When(x => !string.IsNullOrEmpty(x.Slug))
                .MustBeUniqueSlug(_context, x => x.Id)
                .When(x => !string.IsNullOrEmpty(x.Slug));

            RuleFor(x => x.Content)
                .MinimumLength(10).When(x => !string.IsNullOrEmpty(x.Content))
                .WithMessage("Content must be at least 10 characters");
        }
    }
}

