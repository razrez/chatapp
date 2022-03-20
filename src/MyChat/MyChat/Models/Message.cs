using System.ComponentModel.DataAnnotations;

namespace MyChat.Models;

public class Message
{
    public int Id { get; set; }

    [Required]
    public string UserName { get; set; }
    [Required]
    public string Text { get; set; }
    public DateTime When { get; set; }
    
    public string UserId { get; set; }
    public virtual User Sender { get; set; }
    
    public int RoomId { get; set; }
    public Room Room { get; set; }

    public Message()
    {
        When = DateTime.Now;
    }
}