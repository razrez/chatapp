using Microsoft.AspNetCore.Identity;

namespace MyChat.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
        
        public User()
        {
            Messages = new HashSet<Message>();
        }
        public virtual ICollection<Message> Messages { get; set; }

    }
}
