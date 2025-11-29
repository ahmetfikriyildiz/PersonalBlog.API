using FluentValidation;
using PersonalBlog.API.Data;

namespace PersonalBlog.API.Validators.Custom
{
    /// <summary>
    /// Custom validator for checking if a slug is unique in the BlogPosts table
    /// </summary>
    public static class UniqueSlugValidator
    {
        public static IRuleBuilderOptions<T, string> MustBeUniqueSlug<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            PersonalBlogDbContext context,
            int? excludeBlogPostId = null)
        {
            return ruleBuilder
                .Must(slug =>
                {
                    if (string.IsNullOrEmpty(slug))
                        return true;

                    var query = context.BlogPosts
                        .Where(bp => bp.Slug == slug && !bp.IsDeleted);

                    if (excludeBlogPostId.HasValue)
                    {
                        query = query.Where(bp => bp.Id != excludeBlogPostId.Value);
                    }

                    var exists = query.Any();
                    return !exists;
                })
                .WithMessage("A blog post with this slug already exists.");
        }

        public static IRuleBuilderOptions<T, string> MustBeUniqueSlug<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            PersonalBlogDbContext context,
            Func<T, int?> getIdFunc)
        {
            return ruleBuilder
                .Must((instance, slug) =>
                {
                    if (string.IsNullOrEmpty(slug))
                        return true;

                    var excludeId = getIdFunc(instance);
                    var query = context.BlogPosts
                        .Where(bp => bp.Slug == slug && !bp.IsDeleted);

                    if (excludeId.HasValue)
                    {
                        query = query.Where(bp => bp.Id != excludeId.Value);
                    }

                    var exists = query.Any();
                    return !exists;
                })
                .WithMessage("A blog post with this slug already exists.");
        }
    }
}

