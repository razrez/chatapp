using Microsoft.AspNetCore.SignalR;

namespace MyChat.SignalR.Hubs
{
    //будет использоваться как сервис в контроллере
    public class ChatHub : Hub
    {
        /*public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }*/

        public string GetConnectionId() => Context.ConnectionId;
        public Task JoinRoom(string roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }
    }
}
