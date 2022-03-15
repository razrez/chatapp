using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Models;
using MyChat.Rooms;

namespace MyChat.Controllers;

public class ChatController : Controller
{
    private readonly ApplicationContext _applicationContext;
    private readonly RoomsContext _roomsContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;    
    public ChatController(RoomsContext roomsContext, RoleManager<IdentityRole> roleManager, 
        UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext applicationContext)
    {
        _roomsContext = roomsContext;
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
        //var room = await _roomsContext.Rooms.FindAsync(roomId);
        
        return View(messages);
    }

    public async Task<IActionResult> Create(Message message)
    {
        if (ModelState.IsValid)
        {
            message.UserName = User.Identity.Name;
            var sender = await _userManager.GetUserAsync(User);
            message.UserId = sender.Id;
            await _applicationContext.Messages.AddAsync(message);
            await _applicationContext.SaveChangesAsync();
            return Ok();
        }
        return NotFound();
    }
}