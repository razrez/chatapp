using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyChat.Data;
using MyChat.Models;
using MyChat.Rooms;
using MyChat.SignalR.Hubs;

namespace MyChat.Controllers;

// контроллер-мост к клиентам
public class ChatController : Controller
{
    private readonly IHubContext<ChatHub> _chat;

    /*private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager; */   
    public ChatController(ApplicationContext ctx, IHubContext<ChatHub> chat)
    {
        /*_roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;*/
        _chat = chat;
    }
    
    // роутинг такой: Chat/JoinRoom/conId/roomName
    [HttpPost("[action]/{connectionId}/{roomName}")]
    public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
    {
        await _chat.Groups.AddToGroupAsync(connectionId, roomName);
        return Ok();
    }
    
    [HttpPost("[action]/{connectionId}/{roomName}")]
    public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
    { 
        
        await _chat.Groups.RemoveFromGroupAsync(connectionId, roomName);
        return Ok();
    }

    public async Task<IActionResult> SendMessage(int roomId, string message, string roomName, [FromServices] ApplicationContext ctx, [FromServices] UserManager<User> _userManager)
    {
        var sender = await _userManager.FindByNameAsync(User.Identity.Name);
        var currentRoom = await ctx.Rooms.FindAsync(roomId);
        //return Content($"{currentRoom.Id} {sender.UserName}: {message}");
        await ctx.AddAsync(new Message(){Name = sender.Login, Room = currentRoom, Text = message, User = sender});
        await ctx.SaveChangesAsync();
        await _chat.Clients.Group(roomName).SendAsync("ReceiveMessage",message);
        return Ok();
    }
    // GET
    public async Task<IActionResult> Index()
    {
        /*var currentUser = await _userManager.GetUserAsync(User);
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.CurrentUserName = currentUser.UserName;
        }
        var messages = await _applicationContext.Messages.ToListAsync();
        ViewData["messages"] = messages;
        //var room = await _roomsContext.Rooms.FindAsync(roomId);*/
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Send (Message message)
    {
        /*var sender = await _userManager.GetUserAsync(User);
        message.UserId = sender.Id;
        await _applicationContext.Messages.AddAsync(message);
        var res = await _applicationContext.SaveChangesAsync();*/
        return RedirectToAction("Index");
    }
}