using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyChat.Data;
using MyChat.Models;

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
        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }
        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendMessage(string message, string roomName)
        {
            /*//var sender = await _userManager.FindByNameAsync(user.UserName);
            var currentRoom = await ctx.Rooms.FindAsync(roomId);
            //return Content($"{currentRoom.Id} {sender.UserName}: {message}");

            var Mes = new Message() { Name = currentUser.Login, Room = currentRoom!, Text = message, User = currentUser };
            //await ctx.AddAsync(new Message(){Name = sender.Login, Room = currentRoom!, Text = message, User = sender});
            await ctx.AddAsync(Mes);
            await ctx.SaveChangesAsync();*/
        
            await Clients.Group(roomName)
                .SendAsync("ReceiveMessage", message);
        }
    }
}
