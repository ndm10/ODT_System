using Microsoft.AspNetCore.SignalR;

namespace ODT_System.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinGroup(string userId)
        {
            // Tham gia vào group
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task SendMessageToGroup(string userId, string message)
        {
            // Gửi tin nhắn chỉ cho các thành viên trong group
            await Clients.Group(userId).SendAsync("ReceiveMessage", message);
        }
    }
}
