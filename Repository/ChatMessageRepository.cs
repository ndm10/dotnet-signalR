
using signalR.Context;
using signalR.Entity;

namespace signalR.Repository
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly MyDbContext _context;

        public ChatMessageRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddMessage(ChatMessage entity)
        {
            await _context.ChatMessages.AddAsync(entity);
        }

        public async Task UpdateMessage(Guid messageId, bool isReaded)
        {
            var entity = _context.ChatMessages.Find(messageId);

            if (entity != null)
            {
                entity.IsReaded = isReaded;
                _context.Update(entity);
            }
        }
    }
}
