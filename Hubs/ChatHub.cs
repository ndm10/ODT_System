using Microsoft.AspNetCore.SignalR;

namespace ODT_System.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinGroup(int userId, int partnerId)
        {
            // Tạo tên group dựa trên ID của hai người dùng
            // Đảm bảo rằng tên group là duy nhất cho mỗi cặp người dùng bằng cách sắp xếp ID
            var groupName = userId < partnerId ? $"{userId}-{partnerId}" : $"{partnerId}-{userId}";

            // Tham gia vào group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessageToGroup(int userId, int partnerId, string message)
        {
            // Tạo tên group tương tự như khi tham gia group
            var groupName = userId < partnerId ? $"{userId}-{partnerId}" : $"{partnerId}-{userId}";

            // Gửi tin nhắn chỉ cho các thành viên trong group
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
        }
    }
}
