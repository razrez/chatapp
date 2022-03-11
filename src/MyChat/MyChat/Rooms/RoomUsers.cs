using MyChat.Models;
using System.ComponentModel.DataAnnotations;

namespace MyChat.Rooms
{
    public class RoomUsers
    {
        public string? Id { get; set; } //usre's id FROM User:IdentityUser 
        public string? Login { get; set; } //users's login

        public string? RoomId { get; set; } // forign key
        public Room? Room { get; set; } // navigation property
    }
}
