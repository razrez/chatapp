using MyChat.Rooms;

namespace MyChat.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsPrivate { get; set; } = false; // not private by default 

        public string? Password { get; set; }

        public int? AdminId { get; set; } //можно потом сделать форин кей,
                                          //но хз как логика работать будет
                                          //при выходе админа из комнаты может дропнуться вся комната

        public List<RoomUsers> RoomUsers { get; set; } = new();
    }
}
