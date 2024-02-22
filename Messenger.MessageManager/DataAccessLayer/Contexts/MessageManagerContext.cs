using Messenger.MessageManager.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.MessageManager.DataAccessLayer.Contexts;

public class MessageManagerContext : DbContext
{
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<AvailableUser> AvailableUsers { get; set; } = null!;
    
    public MessageManagerContext(DbContextOptions<MessageManagerContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessageManagerContext).Assembly);
    }
}
