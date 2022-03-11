using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyChat.Models;
using MyChat.Rooms;

namespace MyChat.Controllers;

public class RoomsController : Controller
{
    private readonly RoomsContext _context;
    RoleManager<IdentityRole> _roleManager;
    UserManager<User> _userManager;
    
    public RoomsController(RoomsContext context)
    {
        _context = context;
    }
    // GET
    public IActionResult Index()
    {
        var res = _context.RoomUsersContext.ToList();
        
        return Content(res.ToString());
    }
}