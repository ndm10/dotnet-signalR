using Microsoft.AspNetCore.SignalR;

namespace signalR.SignalRHub
{
    public class ChatHub : Hub
    {
        public async Task SendMessageAsync(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
