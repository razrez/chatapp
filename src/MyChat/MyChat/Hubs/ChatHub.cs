using Microsoft.AspNetCore.SignalR;
using MyChat.Models;

namespace MyChat.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(Message message) =>
        await Clients.All.SendAsync("receiveMessage", message);
}