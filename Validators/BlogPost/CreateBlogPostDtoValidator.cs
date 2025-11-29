using FluentValidation;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.BlogPost;
using PersonalBlog.API.Validators.Custom;

namespace PersonalBlog.API.Validators.BlogPost
{
    public class CreateBlogPostDtoValidator : AbstractValidator<CreateBlogPostDto>
    {
        private readonly PersonalBlogDbContext _context;

        public CreateBlogPostDtoValidator(PersonalBlogDbContext context)
        {
            _context = context;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .Length(3, 200).WithMessage("Title must be between 3 and 200 characters");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required")
                .MaximumLength(250).WithMessage("Slug must not exceed 250 characters")
                .MustBeValidSlugFormat()
                .MustBeUniqueSlug(_context);

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MinimumLength(10).WithMessage("Content must be at least 10 characters");
        }
    }
}

