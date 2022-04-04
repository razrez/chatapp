using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyChat.Models;
using MyChat.Models.Rooms;
using MyChat.Rooms;

namespace MyChat.Data;

public class ApplicationContext : IdentityDbContext<User>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<RoomUser> RoomUsers { get; set; }
    public DbSet<RoomConnection> RoomConnections { get; set; }
    

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<RoomUser>()
            .HasKey(x => new { x.RoomId, x.UserId });

        builder.Entity<Message>()
            .HasKey(k => k.Id);

        builder.Entity<RoomConnection>().HasKey(x => new {x.Id, x.RoomId});
        /*builder.Entity<RoomConnection>(table =>
        {
            table.HasKey(x => x.RoomId);
            table.HasOne(x => x.RoomUser)
                .WithMany(x => x.RoomConnections)
                .HasForeignKey(f => f.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        });*/




        /*builder.Entity<Message>()
            .HasOne<User>(u => u.User)
            .WithMany(m => m.Messages)
            .HasForeignKey(u => new { u.UserId,  });*/
    }
}