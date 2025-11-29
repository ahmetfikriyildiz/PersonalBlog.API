using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Contact;

namespace PersonalBlog.API.Services.Interfaces
{
    public interface IContactService
    {
        Task<ResponseContactMessageDto> CreateContactMessageAsync(CreateContactMessageDto dto);
        Task<IEnumerable<ResponseContactMessageDto>> GetAllMessagesAsync();
        Task<PagedResponse<ResponseContactMessageDto>> GetAllMessagesPagedAsync(PaginationFilter filter);
        Task<ResponseContactMessageDto?> GetMessageByIdAsync(int id);
        Task<bool> MarkAsRepliedAsync(int id);
        Task<bool> DeleteMessageAsync(int id);
    }
}

