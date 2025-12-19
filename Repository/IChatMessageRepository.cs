using signalR.Entity;

namespace signalR.Repository
{
    public interface IChatMessageRepository
    {
        public Task AddMessage(ChatMessage entity);
        public Task UpdateMessage(Guid messageId, bool isReaded);
    }
}
