using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    //отображение чата
    [HttpGet("/rooms/{id:int}")]
    [Authorize]
    public IActionResult Chat(int id)
    {
        var roomWithMessages = _applicationContext.Rooms
            .Include(x => x.Messages)
            .FirstOrDefault(x => x.Id == id);
        return View(roomWithMessages);
    }
    
    // логика приссоединения к комнате с выводом сообщений в ней
    [HttpPost]
    public async Task<IActionResult> JoinRoom(int roomId)
    {
        if (User.Identity.IsAuthenticated)
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name); // берем из Identity
            //var currentRoom = await _applicationContext.Rooms.FindAsync(roomId);
            //получим нужную комнату с сообщениями
            /*var roomWithMessages = _applicationContext.Rooms
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == roomId);*/
            
            // просто вход в комнату
            var roomUsers = await _applicationContext.RoomUsers.ToListAsync();
            var roomUser = roomUsers.FirstOrDefault(s => s.UserId == currentUser.Id && s.RoomId == roomId);
            if (roomUser != null)
            {
                return RedirectToAction("Chat", "Rooms", new { id = roomId });
            }

            //добавление нового юзера в комнату
            var user = new RoomUser() { UserId = currentUser.Id, Login = currentUser.Login, RoomId = roomId };
            await _applicationContext.RoomUsers.AddAsync(user);
            await _applicationContext.SaveChangesAsync();

            return RedirectToAction("Chat", "Rooms", new { id = roomId });
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
        var sender = await _userManager.FindByNameAsync(User.Identity.Name);
        var currentRoom = await _applicationContext.Rooms.FindAsync(roomId);
        //return Content($"{currentRoom.Id} {sender.UserName}: {message}");
        await _applicationContext.AddAsync(new Message(){Name = sender.Login, Room = currentRoom, Text = message, User = sender});
        await _applicationContext.SaveChangesAsync();
        
        var roomWithMessages = _applicationContext.Rooms
            .Include(x => x.Messages)
            .FirstOrDefault(x => x.Id == roomId);

        return RedirectToAction("Chat", "Rooms", new { id = roomId });
    }
}