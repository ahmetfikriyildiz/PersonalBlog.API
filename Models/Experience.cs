namespace PersonalBlog.API.Models
{
    public class Experience : BaseEntity
    {
        public string? Company { get; set; }
        public string?Role { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
