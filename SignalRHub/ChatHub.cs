using Microsoft.AspNetCore.SignalR;
using signalR.Context;

namespace signalR.SignalRHub
{
    public class ChatHub : Hub
    {
        // Lưu mapping: userName -> ConnectionId
        private static readonly Dictionary<string, string> _connections = new();
        private readonly MyDbContext _context;

        public ChatHub(MyDbContext context)
        {
            _context = context;
        }

        // Khi client kết nối
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

            // Find user in db
            // If user exists, continue. Else add to db
            var userEntity = await _context.Users.FindAsync(Guid.Parse(userId ?? ""));

            if (userEntity == null)
            {
                userEntity = new Entity.User
                {
                    Id = string.IsNullOrEmpty(userId) ? Guid.NewGuid() : Guid.Parse(userId),
                    Name = "User_" + userId,
                    Logo = "https://www.svgrepo.com/show/452030/avatar-default.svg",
                    Type = 1
                };
                await _context.Users.AddAsync(userEntity);
                await _context.SaveChangesAsync();
            }


            if (!string.IsNullOrEmpty(userId))
            {
                // Nếu user đã tồn tại từ kết nối cũ → xóa cũ
                if (_connections.ContainsKey(userId))
                {
                    _connections.Remove(userId);
                }

                _connections[userId] = Context.ConnectionId;
            }

            await base.OnConnectedAsync();
        }

        // Khi client ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _connections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (user != null)
            {
                _connections.Remove(user);
                await Clients.All.SendAsync("UserOffline", user); // Tùy chọn
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Gửi tin nhắn riêng tư
        public async Task SendMessageAsync(string sender, string receiver, string message)
        {
            if (_connections.TryGetValue(receiver, out var receiverConnectionId))
            {
                // Gửi cho người nhận
                await Clients.Client(receiverConnectionId).SendAsync("ReceivePrivateMessage", sender, message);

                // Gửi lại cho người gửi (để hiển thị ở chatbox của họ)
                await Clients.Caller.SendAsync("ReceivePrivateMessage", sender, message);
            }
            else
            {
                // Nếu người nhận không online → thông báo cho người gửi
                await Clients.Caller.SendAsync("ReceivePrivateMessage", "System", $"User {receiver} đang offline.");
            }
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendGroupMessage(string senderId, string groupName, string message)
        {
            var sender = _context.Users.Find(Guid.Parse(senderId));
            await Clients.Group(groupName).SendAsync("ReceiveGroupMessage", sender, message);
        }

        // Tùy chọn: Lấy danh sách user online
        public async Task GetOnlineUsers()
        {
            var onlineUsers = _connections.Keys.ToList();
            await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);
        }
    }
}
