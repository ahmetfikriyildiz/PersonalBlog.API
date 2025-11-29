using FluentValidation;
using PersonalBlog.API.Data;

namespace PersonalBlog.API.Validators.Custom
{
    /// <summary>
    /// Custom validator for checking if a skill name is unique in the Skills table
    /// </summary>
    public static class UniqueSkillNameValidator
    {
        public static IRuleBuilderOptions<T, string> MustBeUniqueSkillName<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            PersonalBlogDbContext context,
            int? excludeSkillId = null)
        {
            return ruleBuilder
                .Must(name =>
                {
                    if (string.IsNullOrEmpty(name))
                        return true;

                    var query = context.Skills
                        .Where(s => s.Name == name && !s.IsDeleted);

                    if (excludeSkillId.HasValue)
                    {
                        query = query.Where(s => s.Id != excludeSkillId.Value);
                    }

                    var exists = query.Any();
                    return !exists;
                })
                .WithMessage("A skill with this name already exists.");
        }

        public static IRuleBuilderOptions<T, string> MustBeUniqueSkillName<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            PersonalBlogDbContext context,
            Func<T, int?> getIdFunc)
        {
            return ruleBuilder
                .Must((instance, name) =>
                {
                    if (string.IsNullOrEmpty(name))
                        return true;

                    var excludeId = getIdFunc(instance);
                    var query = context.Skills
                        .Where(s => s.Name == name && !s.IsDeleted);

                    if (excludeId.HasValue)
                    {
                        query = query.Where(s => s.Id != excludeId.Value);
                    }

                    var exists = query.Any();
                    return !exists;
                })
                .WithMessage("A skill with this name already exists.");
        }
    }
}

