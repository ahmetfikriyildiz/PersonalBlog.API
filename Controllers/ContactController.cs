using Microsoft.AspNetCore.Mvc;
using PersonalBlog.API.DTOs.Contact;
using PersonalBlog.API.Services.Interfaces;

namespace PersonalBlog.API.Controllers
{
    /// <summary>
    /// Controller for managing contact messages
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Send a contact message (Public endpoint)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseContactMessageDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseContactMessageDto>> SendMessage([FromBody] CreateContactMessageDto dto)
        {
            var createdMessage = await _contactService.CreateContactMessageAsync(dto);
            return CreatedAtAction(nameof(GetMessageById), new { id = createdMessage.Id }, createdMessage);
        }

        /// <summary>
        /// Get all contact messages (Admin endpoint)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ResponseContactMessageDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ResponseContactMessageDto>>> GetAllMessages()
        {
            var messages = await _contactService.GetAllMessagesAsync();
            return Ok(messages);
        }

        /// <summary>
        /// Get contact message by ID (Admin endpoint)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseContactMessageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseContactMessageDto>> GetMessageById(int id)
        {
            var message = await _contactService.GetMessageByIdAsync(id);
            if (message == null)
                return NotFound(new { message = $"Contact message with ID {id} not found." });
            return Ok(message);
        }

        /// <summary>
        /// Mark contact message as replied (Admin endpoint)
        /// </summary>
        [HttpPatch("{id}/mark-replied")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsReplied(int id)
        {
            var result = await _contactService.MarkAsRepliedAsync(id);
            if (!result)
                return NotFound(new { message = $"Contact message with ID {id} not found." });
            return NoContent();
        }

        /// <summary>
        /// Delete a contact message (soft delete - Admin endpoint)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var deleted = await _contactService.DeleteMessageAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Contact message with ID {id} not found." });
            return NoContent();
        }
    }
}

