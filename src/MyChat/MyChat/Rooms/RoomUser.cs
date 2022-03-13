using MyChat.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace MyChat.Rooms
{
    public class RoomUser
    {
        [Column(Order = 0), Key, ForeignKey("Room")]
        public int RoomId { get; set; } // foreign key
        
        //public int Id { get; set; } //just user number
        
        [Key, Column(Order = 1)]
        public string? IdentityUser { get; set; } //user's id FROM User:IdentityUser 
        
        public string? Login { get; set; } //user's login
        
        public Room? Room { get; set; } // navigation property
        
        
        
    }
}
