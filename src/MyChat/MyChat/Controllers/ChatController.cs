using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Data;
using MyChat.Models;
using MyChat.Rooms;

namespace MyChat.Controllers;

public class ChatController : Controller
{
    private readonly ApplicationContext _applicationContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;    
    public ChatController(RoleManager<IdentityRole> roleManager, 
        UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext applicationContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationContext = applicationContext;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.CurrentUserName = currentUser.UserName;
        }
        var messages = await _applicationContext.Messages.ToListAsync();
        ViewData["messages"] = messages;
        //var room = await _roomsContext.Rooms.FindAsync(roomId);
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Send (Message message)
    {
        var sender = await _userManager.GetUserAsync(User);
        message.UserId = sender.Id;
        await _applicationContext.Messages.AddAsync(message);
        var res = await _applicationContext.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}