using Microsoft.EntityFrameworkCore;
using MyChat.Models;
using MyChat.Rooms;

namespace MyChat.Rooms
{
    
    public class RoomsContext : DbContext
    {
        // dbset 
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomUser> RoomUsers { get; set; }
        public RoomsContext( DbContextOptions<RoomsContext> options):base(options)
        {
            Database.EnsureCreated();
            //Database.EnsureDeleted();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Rooms;Username=postgres;Password=3369");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomUser>()
                .HasKey(nameof(RoomUser.RoomId), nameof(RoomUser.IdentityUser));
        }
    }
}
