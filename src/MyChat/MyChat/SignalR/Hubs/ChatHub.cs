using System.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyChat.Data;
using MyChat.Models;
using MyChat.Models.Rooms;

namespace MyChat.SignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;

        public ChatHub(ApplicationContext applicationContext, UserManager<User> userManager)
        {
            _applicationContext = applicationContext;
            _userManager = userManager;
        }

        public string GetConnectionId() => Context.ConnectionId;
        
        public async Task JoinRoom(string roomName, string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("Notify", $"{Context.User?.Identity?.Name} here!");

            var roomIdInt = int.Parse(roomId);
            var roomConnection = new RoomConnection()
            {
                RoomId = roomIdInt,
                RoomName = roomName,
                UserLogin = Context.User?.Identity?.Name,
                ConnectionId = Context.ConnectionId
            };
            if (_applicationContext.RoomConnections.Contains(roomConnection))
            {
                _applicationContext.Update(roomConnection);
            }
            else
            {
                _applicationContext.RoomConnections.Add(roomConnection);
            }
            await _applicationContext.SaveChangesAsync();
        }
        public async Task Kick(string roomName, string roomId, string userName)
        {
            await Clients.OthersInGroup(roomName).SendAsync("Notify", $"{userName} was kicked!");
            
            var roomIdInt = int.Parse(roomId);
            var connectionId = QueryableExtensions
                .Include(_applicationContext.RoomConnections,
                    connection => connection.ConnectionId).AsSplitQuery()
                .First(c => c.RoomId == roomIdInt && c.UserLogin == userName).ConnectionId;
            
            // redirect and close connection
            await Clients.Client(connectionId).SendAsync("By", "u've been banned, bro");
            //await Groups.RemoveFromGroupAsync(connectionId, roomName);
            
            var currentUser = await _userManager.FindByNameAsync(userName); 
            
            var roomUser = _applicationContext.RoomUsers.AsQueryable()
                .FirstOrDefault(s => s.UserId == currentUser.Id && s.RoomId == roomIdInt);
            _applicationContext.RoomUsers.Remove(roomUser);
            await _applicationContext.SaveChangesAsync();
            
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

        /*public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.Group("").SendAsync("Notify", $"{Context.User?.Identity?.Name} has left :|");
            await base.OnDisconnectedAsync(exception);
        }*/
    }
}
