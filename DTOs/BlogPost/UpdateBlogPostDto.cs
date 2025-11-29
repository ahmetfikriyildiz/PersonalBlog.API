namespace PersonalBlog.API.DTOs.BlogPost
{
    public class UpdateBlogPostDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Slug { get; set; }
        public string? Content { get; set; }
        public bool? IsPublished { get; set; }
    }
}

