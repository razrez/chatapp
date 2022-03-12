using MyChat.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace MyChat.Rooms
{
    public class RoomUsers
    {
        public int Id { get; set; } //just user number
        
        public int? RoomId { get; set; } // foreign key
        public Room? Room { get; set; } // navigation property
        public string? IdentityUser { get; set; } //user's id FROM User:IdentityUser 
        public string? Login { get; set; } //user's login
        
        
    }
}
