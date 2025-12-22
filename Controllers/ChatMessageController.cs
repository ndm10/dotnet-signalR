using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using signalR.Context;
using signalR.Entity;

namespace signalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ChatMessageController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages(string groupName, int page)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                return Ok(new List<ChatMessage>());
            }

            var chatMessages = _context.ChatMessages;
            var users = _context.Users;

            var messages = chatMessages
                .Join(users,
                    l => l.SenderId,
                    users => users.Id,
                    (chatMessages, users) => new { chatMessages, users }
                )
                .OrderByDescending(x => x.chatMessages.Timestamp)
                .Where(x => x.chatMessages.GroupName == groupName)
                .Skip((page - 1) * 10)
                .Take(10)
                .Select(
                    x => new
                    {
                        x.chatMessages.SenderId,
                        x.chatMessages.Message,
                        x.chatMessages.Timestamp,
                        x.users.Name,
                        x.users.Logo
                    }
                )
                .ToList();

            return Ok(messages.OrderBy(x => x.Timestamp));
        }
    }
}
