namespace MyChat.Models
{
    public class Room
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPrivate { get; set; }

        public HashCode? Password { get; set; }

        public string? AdminId;
    }
}
