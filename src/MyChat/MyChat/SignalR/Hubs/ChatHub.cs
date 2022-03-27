using System.Security.Claims;
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
        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }
        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendMessage(string message, string roomName, string roomId)
        {
            await Clients.Group(roomName)
                .SendAsync("ReceiveMessage", message,Context.User?.Identity?.Name,DateTime.Now);
            
            var sender = await _userManager.FindByNameAsync(Context.User?.Identity?.Name);
            var roomIdInt = Int32.Parse(roomId);
            var currentRoom = await _applicationContext.Rooms.FindAsync(roomIdInt);
            //return Content($"{currentRoom.Id} {sender.UserName}: {message}");
            
            var mesDb = new Message() { Name = sender.UserName, Room = currentRoom!, Text = message, User = sender };
            //await ctx.AddAsync(new Message(){Name = sender.Login, Room = currentRoom!, Text = message, User = sender});
            await _applicationContext.AddAsync(mesDb);
            await _applicationContext.SaveChangesAsync();
            
        }

    }
}
