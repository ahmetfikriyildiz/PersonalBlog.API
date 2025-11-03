using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.API.DTOs.Contact
{
    public class CreateContactMessageDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 2000 characters")]
        public string Message { get; set; } = string.Empty;
    }
}
