using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Shared.DTOs;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly SmartInventoryDbContext _context;

        public ChatController(SmartInventoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMessages()
        {
            var messages = _context.ChatMessages
                .OrderByDescending(m => m.Timestamp)
                .Take(50)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    Sender = m.Sender,
                    Message = m.Message,
                    Timestamp = m.Timestamp
                })
                .ToList();

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMessage([FromBody] ChatMessageDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Sender) || string.IsNullOrWhiteSpace(dto.Message))
                return BadRequest("Sender and Message are required.");

            var chatMessage = new ChatMessage
            {
                Sender = dto.Sender,
                Message = dto.Message,
                Timestamp = DateTime.UtcNow
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Message saved successfully", chatMessage.Id });
        }
    }
}
