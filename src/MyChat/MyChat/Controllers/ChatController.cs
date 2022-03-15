using Microsoft.AspNetCore.Mvc;

namespace MyChat.Controllers;

public class ChatController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}