using PersonalBlog.API.DTOs.Contact;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;
using PersonalBlog.API.Services.Interfaces;

namespace PersonalBlog.API.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<ResponseContactMessageDto> CreateContactMessageAsync(CreateContactMessageDto dto)
        {
            var contactMessage = new ContactMessage
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Subject = dto.Subject,
                Message = dto.Message,
                ReceivedAt = DateTime.UtcNow,
                IsReplied = false,
                CreatedAt = DateTime.UtcNow
            };

            await _contactRepository.CreateAsync(contactMessage);

            return await _contactRepository.GetMessageByIdDtoAsync(contactMessage.Id)
                ?? throw new InvalidOperationException("Failed to retrieve created contact message.");
        }

        public async Task<IEnumerable<ResponseContactMessageDto>> GetAllMessagesAsync()
        {
            return await _contactRepository.GetAllMessagesDtoAsync();
        }

        public async Task<ResponseContactMessageDto?> GetMessageByIdAsync(int id)
        {
            return await _contactRepository.GetMessageByIdDtoAsync(id);
        }

        public async Task<bool> MarkAsRepliedAsync(int id)
        {
            return await _contactRepository.MarkAsRepliedAsync(id);
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            return await _contactRepository.DeleteAsync(id);
        }
    }
}

