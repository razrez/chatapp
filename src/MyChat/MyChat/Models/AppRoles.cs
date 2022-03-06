using Microsoft.AspNetCore.Identity;
namespace MyChat.Models
{
    public class AppRoles : IdentityRole
    {
        public AppRoles() : base() { }
        public AppRoles(string name)
            : base(name)
        { }
    }
}
