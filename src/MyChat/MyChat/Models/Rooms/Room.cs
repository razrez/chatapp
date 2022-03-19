using MyChat.Models.Rooms;
using MyChat.Rooms;

namespace MyChat.Models
{
    public class Room
    {

        public string AdminId { get; set; } //можно потом сделать форин кей,
                                          //но хз как логика работать будет
                                          //при выходе админа из комнаты может дропнуться вся комната
        
        public Room()
        {
            Messages = new List<Message>();
            RoomUsers = new List<RoomUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public RoomType Type { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<RoomUser> RoomUsers { get; set; }
    }
}
