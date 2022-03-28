using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyChat.Controllers;
using MyChat.Data;
using MyChat.Models;

namespace MyChat.SignalR.Hubs
{
    //будет использоваться как сервис в контроллере
    public class ChatHub : Hub
    {
        private readonly ApplicationContext _applicationContext;
        private readonly UserManager<User> _userManager;

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
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("Notify", $"{Context.User?.Identity?.Name} here!");
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
}
