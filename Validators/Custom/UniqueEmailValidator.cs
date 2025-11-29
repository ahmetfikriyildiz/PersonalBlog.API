using FluentValidation;
using PersonalBlog.API.Data;

namespace PersonalBlog.API.Validators.Custom
{
    /// <summary>
    /// Custom validator for checking if an email is unique in the Users table
    /// </summary>
    public static class UniqueEmailValidator
    {
        public static IRuleBuilderOptions<T, string> MustBeUniqueEmail<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            PersonalBlogDbContext context,
            int? excludeUserId = null)
        {
            return ruleBuilder
                .Must(email =>
                {
                    if (string.IsNullOrWhiteSpace(email))
                        return true;

                    var query = context.Users
                        .Where(u => u.Email == email && !u.IsDeleted);

                    if (excludeUserId.HasValue)
                    {
                        query = query.Where(u => u.Id != excludeUserId.Value);
                    }

                    var exists = query.Any();
                    return !exists;
                })
                .WithMessage("A user with this email already exists.");
        }
    }
}

