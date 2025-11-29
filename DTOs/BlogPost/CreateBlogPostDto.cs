namespace PersonalBlog.API.DTOs.BlogPost
{
    public class CreateBlogPostDto
    {
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsPublished { get; set; } = false;
    }
}
