using Imdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Imdentity.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly UserManager

        public AdminController(ApplicationDbContext context, 
            RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _signInManager = signInManager; 
            _userManager = userManager; 
        }
        public IActionResult Index()
        {   
            var users = _context.Users.ToList();
            return _signInManager.Context.User.Identity.Name switch
            {
                "admin@mail.ru" => View(users),
                _ => RedirectToAction("Index", "Home")
            };
        }

        public async Task<IActionResult> GetRoles()
        {
            //await _roleManager.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN"});
            //await _roleManager.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER"});
            //this space for working with DataBase
            //

            /*var users = await _context.Users.ToListAsync();
            await Task.Run(async () =>
            {
                foreach(var user in users)
                {
                    await _userManager.DeleteAsync(user);
                }
            });*/

            //я создал две роли админ и юзер
            // по дефолу во время реги ты становишься просто "User"
            //хочу релизовать, чтобы при создании комнаты - создатель-юзер становился ее админом 
            var roles = _roleManager.Roles.ToArrayAsync().Result;
            return View(roles);
        }
    }   

}
