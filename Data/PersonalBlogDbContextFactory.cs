using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace PersonalBlog.API.Data
{
    public class PersonalBlogDbContextFactory : IDesignTimeDbContextFactory<PersonalBlogDbContext>
    {
        public PersonalBlogDbContext CreateDbContext(string[] args)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("Default");

            // Build DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<PersonalBlogDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new PersonalBlogDbContext(optionsBuilder.Options);
        }
    }
}

