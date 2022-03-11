using Microsoft.EntityFrameworkCore;
using MyChat.Models;
using MyChat.Rooms;

namespace MyChat.Rooms
{
    
    public class RoomsContext : DbContext
    {
        // dbset 
        public DbSet<Room> RoomContext { get; set; }
        public DbSet<RoomUsers> RoomUsersContext { get; set; }
        public RoomsContext( DbContextOptions<RoomsContext> options):base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Rooms;Username=postgres;Password=3369");
        }
    }
}
