using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Models;
using MyChat.Rooms;
using MyChat.RoomsViewModels;

namespace MyChat.Controllers;


public class RoomsController : Controller
{
    private readonly ApplicationContext _applicationContext;
    private readonly RoomsContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;    
    public RoomsController(RoomsContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
        SignInManager<User> signInManager, ApplicationContext applicationContext)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationContext = applicationContext;
    }
    // GET
    public IActionResult Index()
    {
        var rooms = _context.Rooms.ToList();
        return View(rooms);
    }
    //join(get, post), leave, create(get,post)

    [HttpPost]
    public async Task<IActionResult> Join(int roomId)
    {
        //нужно будет добавить проверку на наличие юзера в комнате
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name); // берем из Identity
            var currentRoom = await _context.Rooms.FindAsync(roomId);

            //var roomsUsers = await _context.RoomUser.ToListAsync();
            //var roomUser = await _context.RoomUser.FindAsync(currentUser.Id)
            //вот тут косяк, ищется по PK, а тут это Id from Identity
            var roomUsers = await _context.RoomUsers.ToListAsync();
            var roomUser = roomUsers.FirstOrDefault(s => s.IdentityUser == currentUser.Id && s.RoomId == roomId);
            if (roomUser != null)
            {
                return View(currentRoom);
            }

            //добавление юзера в комнату
            var user = new RoomUser() { IdentityUser = currentUser.Id, Login = currentUser.Login, RoomId = roomId };
            await _context.RoomUsers.AddAsync(user);
            await _context.SaveChangesAsync();

            return View(currentRoom);
        }
       
        return RedirectToAction("Login","Account");
    }

    public async Task<IActionResult> Room()
    {
        /*var room = await _context.Rooms.FindAsync(roomId);
        return Content(String.Join(",", room.RoomUsers ));*/
        return View();
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoomModel roomModel)
    {
        //добавить проверку на уже существующую комнату
        if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Account");
        if (ModelState.IsValid)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var room = new Room() { Name = roomModel.Name, IsPrivate = roomModel.IsPrivate, AdminId = currentUser.Id };
            var user = new RoomUser() { IdentityUser = currentUser.Id, Login = currentUser.Login, Room = room };
            await _context.Rooms.AddAsync(room);
            await _context.RoomUsers.AddAsync(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
        return View(roomModel);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id) //room-id
    {
        if (!_signInManager.IsSignedIn(User))
        {
           return RedirectToAction("Login", "Account");
        }
        var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
        var currentRoom = await _context.Rooms.FindAsync(id);
        if (currentUser.Id == currentRoom.AdminId)
        {
            _context.Remove(currentRoom);
            var res = _context.SaveChangesAsync();
            if (!res.IsCompletedSuccessfully) return RedirectToAction("Index");
        }
        else
        {
            return Content("YOU ARE NOT A ROOM-ADMIN!!");
        }

        return Content("something has gone wrong...");
    }
}