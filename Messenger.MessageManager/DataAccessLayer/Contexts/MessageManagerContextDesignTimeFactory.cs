using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Messenger.MessageManager.DataAccessLayer.Contexts;

public class MessageManagerContextDesignTimeFactory : IDesignTimeDbContextFactory<MessageManagerContext>
{
    public MessageManagerContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MessageManagerContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=messenger_message_manager_db;Username=postgres;Password=password");
        return new MessageManagerContext(optionsBuilder.Options);
    }
}