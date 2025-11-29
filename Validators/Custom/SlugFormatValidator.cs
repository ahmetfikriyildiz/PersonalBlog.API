using FluentValidation;

namespace PersonalBlog.API.Validators.Custom
{
    /// <summary>
    /// Custom validator for validating slug format (lowercase alphanumeric with hyphens)
    /// </summary>
    public static class SlugFormatValidator
    {
        private const string SlugPattern = @"^[a-z0-9]+(?:-[a-z0-9]+)*$";

        public static IRuleBuilderOptions<T, string> MustBeValidSlugFormat<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(SlugPattern)
                .WithMessage("Slug must be lowercase alphanumeric with hyphens only");
        }
    }
}

