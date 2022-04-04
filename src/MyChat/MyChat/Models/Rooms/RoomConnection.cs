using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyChat.Rooms;

namespace MyChat.Models.Rooms;

public class RoomConnection
{
    public int Id { get; set; } // foreign key
    
    public int RoomId { get; set; }
    
    public string RoomName { get; set; }
    public Room? Room { get; set; } // navigation property
    
    public string? UserLogin { get; set; } 
    
    public string? ConnectionId { get; set; }
}