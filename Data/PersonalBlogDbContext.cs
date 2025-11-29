using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Data
{
    public class PersonalBlogDbContext : DbContext
    {
        public PersonalBlogDbContext(DbContextOptions<PersonalBlogDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<ProjectSkill> ProjectSkills => Set<ProjectSkill>();
        public DbSet<Education> Educations => Set<Education>();
        public DbSet<Experience> Experiences => Set<Experience>();
        public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new SkillConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectSkillConfiguration());
            modelBuilder.ApplyConfiguration(new EducationConfiguration());
            modelBuilder.ApplyConfiguration(new ExperienceConfiguration());
            modelBuilder.ApplyConfiguration(new BlogPostConfiguration());
            modelBuilder.ApplyConfiguration(new ContactMessageConfiguration());

            // Soft delete global filter (opsiyonel)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(PersonalBlogDbContext)
                        .GetMethod(nameof(ApplyIsDeletedFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }
        }

        private static void ApplyIsDeletedFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }
    }
    public class ProjectSkillConfiguration : IEntityTypeConfiguration<ProjectSkill>
    {
        public void Configure(EntityTypeBuilder<ProjectSkill> builder)
        {
            builder.ToTable("ProjectSkills");
            builder.HasKey(x => new { x.ProjectId, x.SkillId });
        }
    }
    public class SkillConfiguration : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(80);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.Property(x => x.Category).HasMaxLength(60);
            builder.Property(x => x.Proficiency).IsRequired();

            builder.HasMany(x => x.ProjectSkills)
                   .WithOne(x => x.Skill)
                   .HasForeignKey(x => x.SkillId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.Property(x => x.Title).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Slug).HasMaxLength(180);
            builder.HasIndex(x => x.Slug).IsUnique(false); // İstersen unique yap
            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.Property(x => x.GitHubUrl).HasMaxLength(400);
            builder.Property(x => x.LiveUrl).HasMaxLength(400);

            builder.HasMany(x => x.ProjectSkills)
                   .WithOne(x => x.Project)
                   .HasForeignKey(x => x.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(x => x.FullName).IsRequired().HasMaxLength(120);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Title).HasMaxLength(100);
            builder.Property(x => x.AvatarUrl).HasMaxLength(400);

            builder.HasMany(x => x.Projects)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.BlogPosts)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Educations)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Experiences)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class EducationConfiguration : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.ToTable("Educations");

            builder.Property(x => x.School).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Degree).IsRequired().HasMaxLength(150);
            builder.Property(x => x.FieldOfStudy).HasMaxLength(150);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Educations)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
    {
        public void Configure(EntityTypeBuilder<Experience> builder)
        {
            builder.ToTable("Experiences");

            builder.Property(x => x.Company).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Role).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Location).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(2000);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Experiences)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.ToTable("BlogPosts");

            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(250);
            builder.HasIndex(x => x.Slug).IsUnique();
            builder.Property(x => x.Content).IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.BlogPosts)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
    {
        public void Configure(EntityTypeBuilder<ContactMessage> builder)
        {
            builder.ToTable("ContactMessages");

            builder.Property(x => x.FullName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Subject).HasMaxLength(300);
            builder.Property(x => x.Message).IsRequired().HasMaxLength(5000);
        }
    }

}
