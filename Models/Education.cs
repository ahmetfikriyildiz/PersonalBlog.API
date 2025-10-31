namespace PersonalBlog.API.Models
{
    public class Education : BaseEntity
    {
        public string School { get; set; }
        public string Degree { get; set; }
        public string? FieldOfStudy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
