using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Data;
using MyChat.Models;
using MyChat.Rooms;
using MyChat.RoomsViewModels;

namespace MyChat.Controllers;


public class RoomsController : Controller
{
    private readonly ApplicationContext _applicationContext;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;    
    public RoomsController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
        SignInManager<User> signInManager, ApplicationContext applicationContext)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
        _applicationContext = applicationContext;
    }
    
    // просто отображаются все комнаты
    public IActionResult Index()
    {
        var rooms = _applicationContext.Rooms.ToList();
        return View(rooms);
    }
    //join(post), leave, create(get,post)

    // логика приссоединения к комнате с выводом сообщений в ней
    [HttpPost]
    public async Task<IActionResult> Join(int roomId)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name); // берем из Identity
            var currentRoom = await _applicationContext.Rooms.FindAsync(roomId);
            
            // просто вход в комнату
            var roomUsers = await _applicationContext.RoomUsers.ToListAsync();
            var roomUser = roomUsers.FirstOrDefault(s => s.UserId == currentUser.Id && s.RoomId == roomId);
            if (roomUser != null)
            {
                return View(currentRoom);
            }

            //добавление нового юзера в комнату
            var user = new RoomUser() { UserId = currentUser.Id, Login = currentUser.Login, RoomId = roomId };
            await _applicationContext.RoomUsers.AddAsync(user);
            await _applicationContext.SaveChangesAsync();

            return View(currentRoom);
        }
       
        return RedirectToAction("Login","Account");
    }
    
    // ссылка в индексе на создание комнаты
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
            var room = new Room() { Name = roomModel.Name, AdminId = currentUser.Id };
            var user = new RoomUser() { UserId = currentUser.Id, Login = currentUser.Login, Room = room };
            await _applicationContext.Rooms.AddAsync(room);
            await _applicationContext.RoomUsers.AddAsync(user);
            await _applicationContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(roomModel);
    }

    // удаление комнаты через индекс
    [HttpPost]
    public IActionResult Delete(int id) //room-id
    {
        if (!_signInManager.IsSignedIn(User))
        {
           return RedirectToAction("Login", "Account");
        }
        
        var currentRoom = _applicationContext.Rooms.Find(id);
        var currentUser = _applicationContext.Users.First(us => us.UserName == User.Identity.Name);

        if (currentRoom.AdminId == currentUser.Id)
        {
            _applicationContext.Rooms.Remove(currentRoom);
            _applicationContext.SaveChanges();
            return RedirectToAction("Index");
        }

        return Content("ACCESS DENIED");

    }
    
    // отправка сообщения после приссоединения
    [HttpPost]
    public async Task<IActionResult> Send (int roomId, string message)
    {
        var sender = await _userManager.GetUserAsync(User);
        var currentRoom = await _applicationContext.Rooms.FindAsync(roomId);

        return Content($"{roomId} {User.Identity.Name}: {message}");
        /*var messdb = new Message(){UserName = sender.UserName, Text = message, Sender = sender, Room=currentRoom};
        await _applicationContext.Messages.AddAsync(messdb);
        var res = await _applicationContext.SaveChangesAsync();
        return View("Join",await _applicationContext.Rooms.FindAsync(roomId));*/
    }
}