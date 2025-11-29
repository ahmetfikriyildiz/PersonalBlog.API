namespace PersonalBlog.API.DTOs.Users
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? AvatarUrl { get; set; }
        public string? GithubUrl { get; set; }
        public string? LinkedinUrl { get; set; }
    }
}

