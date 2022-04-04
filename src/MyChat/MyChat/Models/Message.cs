using System.ComponentModel.DataAnnotations;
using MyChat.Rooms;

namespace MyChat.Models;

public class Message
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public string Name { get; set; }
    public string Text { get; set; }
    public DateTime When { get; set; }
    public Message()
    {
        When = DateTime.Now;
    }
}