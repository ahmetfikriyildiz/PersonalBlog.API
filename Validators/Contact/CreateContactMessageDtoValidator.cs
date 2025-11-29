using FluentValidation;
using PersonalBlog.API.DTOs.Contact;

namespace PersonalBlog.API.Validators.Contact
{
    public class CreateContactMessageDtoValidator : AbstractValidator<CreateContactMessageDto>
    {
        public CreateContactMessageDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(120).WithMessage("Full name must not exceed 120 characters")
                .Matches(@"^[\p{L}\s'-]+$").WithMessage("Full name can only contain letters, spaces, hyphens, and apostrophes");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(200).WithMessage("Email must not exceed 200 characters");

            RuleFor(x => x.Subject)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Subject))
                .WithMessage("Subject must not exceed 200 characters");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required")
                .Length(10, 2000).WithMessage("Message must be between 10 and 2000 characters");
        }
    }
}

