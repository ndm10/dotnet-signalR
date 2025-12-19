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

        [HttpGet("get-messages/{GroupName}/{page}")]
        public async Task<IActionResult> GetMessages(string GroupName, int page)
        {
            if (string.IsNullOrWhiteSpace(GroupName))
            {
                return Ok(new List<ChatMessage>());
            }

            var messages = _context.ChatMessages
                .Where(m => m.GroupName == GroupName)
                .OrderBy(m => m.Timestamp)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToList();

            return Ok(messages);
        }
    }
}
