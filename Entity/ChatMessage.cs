namespace signalR.Entity
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public bool IsReaded { get; set; } = false;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
