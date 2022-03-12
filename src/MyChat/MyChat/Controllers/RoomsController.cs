using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyChat.Models;
using MyChat.Rooms;
using MyChat.RoomsViewModels;

namespace MyChat.Controllers;

public class RoomsController : Controller
{
    private readonly RoomsContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;    
    public RoomsController(RoomsContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    // GET
    public IActionResult Index()
    {
        var rooms = _context.Rooms.ToList();
        return View(rooms);
    }
    //join(get, post), leave, create(get,post)

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoomModel roomModel)
    {
        if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
        if (ModelState.IsValid)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var room = new Room() { Name = roomModel.Name, IsPrivate = roomModel.IsPrivate, AdminId = currentUser.Id };
            var user = new RoomUsers() { IdentityUser = currentUser.Id, Login = currentUser.Login, Room = room };
            await _context.Rooms.AddAsync(room);
            await _context.RoomUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
        return View(roomModel);
    }

}