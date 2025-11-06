using PersonalBlog.API.DTOs.Contact;
using PersonalBlog.API.Models;

namespace PersonalBlog.API.Repositories.Interfaces
{
    public interface IContactRepository : IRepository<ContactMessage>
    {
        Task<IEnumerable<ResponseContactMessageDto>> GetAllMessagesDtoAsync();
        Task<ResponseContactMessageDto?> GetMessageByIdDtoAsync(int id);
        Task<bool> MarkAsRepliedAsync(int id);
    }
}

