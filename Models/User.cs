namespace PersonalBlog.API.Models
{
    public class User : BaseEntity
    {
        public string? FullName { get; set; }
        public string? Email { get; set; } 
        public string? PasswordHash { get; set; } 
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<Experience> Experiences { get; set; } = new List<Experience>();
    }
}
