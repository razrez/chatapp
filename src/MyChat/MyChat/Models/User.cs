using Microsoft.AspNetCore.Identity;

namespace MyChat.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }

    }
}
