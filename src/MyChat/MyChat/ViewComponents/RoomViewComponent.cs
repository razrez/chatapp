using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyChat.Data;
using MyChat.Models.Rooms;

namespace MyChat.ViewComponents;

public class RoomViewComponent : ViewComponent
{
    private readonly ApplicationContext _ctx;

    public RoomViewComponent(ApplicationContext ctx)
    {
        _ctx = ctx;
    }

    // чаты, к которым юзер уже приссоединился
    public IViewComponentResult Invoke()
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = User;
        var chats = _ctx.RoomUsers
            .Include(x => x.Room)
            .Where(x => x.UserId == userId
                        && x.Room.Type == RoomType.Room)
            .Select(x => x.Room)
            .ToList();

        return View(chats);
    }
}