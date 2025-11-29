using FluentValidation;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Auth;
using PersonalBlog.API.Validators.Custom;

namespace PersonalBlog.API.Validators.Auth
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        private readonly PersonalBlogDbContext _context;

        public RegisterDtoValidator(PersonalBlogDbContext context)
        {
            _context = context;

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .Length(2, 120).WithMessage("Full name must be between 2 and 120 characters")
                .Matches(@"^[\p{L}\s'-]+$").WithMessage("Full name can only contain letters, spaces, hyphens, and apostrophes");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters")
                .MustBeUniqueEmail(_context);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}

