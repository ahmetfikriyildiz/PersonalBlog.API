namespace PersonalBlog.API.Models
{
    public class BlogPost : BaseEntity
    {
        public string? Title { get; set; }
        public string? Slug { get; set; } // Unique
        public string? Content { get; set; }
        public bool IsPublished { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
