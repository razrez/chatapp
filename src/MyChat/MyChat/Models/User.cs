using Microsoft.AspNetCore.Identity;
using MyChat.Rooms;

namespace MyChat.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
        
        public User()
        {
            Messages = new HashSet<Message>();
            RoomUsers = new List<RoomUser>();
        }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<RoomUser> RoomUsers { get; set; }

    }
}
