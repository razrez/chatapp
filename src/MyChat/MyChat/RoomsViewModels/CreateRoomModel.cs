using System.ComponentModel.DataAnnotations;
using MyChat.Rooms;

namespace MyChat.RoomsViewModels;

public class CreateRoomModel
{
    //public int IdentityUser { get; set; }
    
    [Required]
    [Display(Name = "Room Name")]
    public string Name { get; set; }

    public bool IsPrivate { get; set; } = false; // not private by default 

    public string? Password { get; set; }

    //public List<RoomUsers> RoomUsers { get; set; } = new();
}