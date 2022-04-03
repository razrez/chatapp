using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MyChat.Data;
using MyChat.Models;

namespace MyChat.SignalR.Hubs
{
    //будет использоваться как сервис в контроллере
    public class ChatHub : Hub
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;
        private Dictionary<string, UserInfo> connections = new();

        //key-connectionId; value-roomName
        public ChatHub(ApplicationContext applicationContext, UserManager<User> userManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
        }
        /*public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }*/

        public string GetConnectionId() => Context.ConnectionId;
        public async Task JoinRoom(string roomName)
        {
            connections[roomName] = new UserInfo(Context.User?.Identity?.Name!,Context.ConnectionId);
            var id = connections
                .First(k => k.Key == roomName && k.Value.UserName == Context.User?.Identity?.Name!)
                .Value.ConnectionId;
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("Notify", $"{id} here!");
        }
        public async Task Kick(string room, string userName)
        {
            
            /*var id = connections
                            .First(k => k.Key == roomName && k.Value.UserName == userName)
                            .Value.ConnectionId;*/
            await Clients.Group(room).SendAsync("Notify", $" was kicked!");
            
            //await Groups.RemoveFromGroupAsync(id, roomName);

        }
        
        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendMessage(string message, string roomName, string roomId)
        {
            await Clients.Group(roomName)
                .SendAsync("ReceiveMessage", message,Context.User?.Identity?.Name,DateTime.Now.ToString("h:mm:ss"));
            
            var sender = await _userManager.FindByNameAsync(Context.User?.Identity?.Name);
            var roomIdInt = int.Parse(roomId);
            var currentRoom = await _applicationContext.Rooms.FindAsync(roomIdInt);
            //return Content($"{currentRoom.Id} {sender.UserName}: {message}");
            
            var mesDb = new Message() { Name = sender.UserName, Room = currentRoom!, Text = message, User = sender };
            //await ctx.AddAsync(new Message(){Name = sender.Login, Room = currentRoom!, Text = message, User = sender});
            await _applicationContext.AddAsync(mesDb);
            await _applicationContext.SaveChangesAsync();
            
        }

        /*public override async Task OnConnectedAsync()
        {
            var vals = connections.GetValueOrDefault(Context.ConnectionId);
            await Clients.All.SendAsync("Notify", $"{vals} here!");
            await base.OnConnectedAsync();
        }*/

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Group("").SendAsync("Notify", $"{Context.User?.Identity?.Name} has left :|");
            await base.OnDisconnectedAsync(exception);
        }
    }

    public class UserInfo
    {
        public UserInfo(string userName, string connectionId)
        {
            UserName = userName;
            ConnectionId = connectionId;
        }

        public string UserName { get; }
        public string ConnectionId { get; }
    }

}
