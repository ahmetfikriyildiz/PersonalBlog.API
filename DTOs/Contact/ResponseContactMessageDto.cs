namespace PersonalBlog.API.DTOs.Contact
{
    public class ResponseContactMessageDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Subject { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ReceivedAt { get; set; }
        public bool IsReplied { get; set; }
    }
}
