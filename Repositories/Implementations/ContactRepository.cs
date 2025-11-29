using Microsoft.EntityFrameworkCore;
using PersonalBlog.API.Data;
using PersonalBlog.API.DTOs.Commons;
using PersonalBlog.API.DTOs.Contact;
using PersonalBlog.API.Models;
using PersonalBlog.API.Repositories.Interfaces;

namespace PersonalBlog.API.Repositories.Implementations
{
    public class ContactRepository : Repository<ContactMessage>, IContactRepository
    {
        public ContactRepository(PersonalBlogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ResponseContactMessageDto>> GetAllMessagesDtoAsync()
        {
            return await _context.ContactMessages
                .Where(cm => !cm.IsDeleted)
                .OrderByDescending(cm => cm.ReceivedAt)
                .Select(cm => new ResponseContactMessageDto
                {
                    Id = cm.Id,
                    FullName = cm.FullName,
                    Email = cm.Email,
                    Subject = cm.Subject,
                    Message = cm.Message,
                    ReceivedAt = cm.ReceivedAt,
                    IsReplied = cm.IsReplied
                })
                .ToListAsync();
        }

        public async Task<PagedResponse<ResponseContactMessageDto>> GetAllMessagesDtoPagedAsync(PaginationFilter filter)
        {
            var query = _context.ContactMessages.Where(cm => !cm.IsDeleted);
            var totalRecords = await query.CountAsync();

            var pagedData = await query
                .OrderByDescending(cm => cm.ReceivedAt)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(cm => new ResponseContactMessageDto
                {
                    Id = cm.Id,
                    FullName = cm.FullName,
                    Email = cm.Email,
                    Subject = cm.Subject,
                    Message = cm.Message,
                    ReceivedAt = cm.ReceivedAt,
                    IsReplied = cm.IsReplied
                })
                .ToListAsync();

            return new PagedResponse<ResponseContactMessageDto>(pagedData, filter.PageNumber, filter.PageSize, totalRecords);
        }

        public async Task<ResponseContactMessageDto?> GetMessageByIdDtoAsync(int id)
        {
            return await _context.ContactMessages
                .Where(cm => cm.Id == id && !cm.IsDeleted)
                .Select(cm => new ResponseContactMessageDto
                {
                    Id = cm.Id,
                    FullName = cm.FullName,
                    Email = cm.Email,
                    Subject = cm.Subject,
                    Message = cm.Message,
                    ReceivedAt = cm.ReceivedAt,
                    IsReplied = cm.IsReplied
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> MarkAsRepliedAsync(int id)
        {
            var message = await GetByIdAsync(id);
            if (message == null)
                return false;

            message.IsReplied = true;
            message.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

