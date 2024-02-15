using Messenger.MessageManager.DataAccessLayer.Contexts;
using Messenger.MessageManager.DataAccessLayer.Models;
using Messenger.MessageManager.DataAccessLayer.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.MessageManager.DataAccessLayer.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly MessageManagerContext _context;

    public MessageRepository(MessageManagerContext context)
    {
        _context = context;
    }


/// <summary>
/// Retrieves unread messages by recipient ID and marks them as read
/// </summary>
/// <param name="recipientId">The ID of the recipient</param>
/// <param name="cancellationToken">The token to monitor for cancellation requests</param>
/// <returns>A collection of unread messages for the recipient</returns>
public async Task<IEnumerable<Message>> GetAndMarkAsReadByRecipient(Guid recipientId, CancellationToken cancellationToken)
{
    // Retrieve unread messages
    var messages = _context.Messages.Where(m => m.RecipientId == recipientId && !m.IsRead);

    // Materialize the query and retrieve the messages
    var response = await messages.ToListAsync(cancellationToken);

    // Mark the retrieved messages as read
    foreach (var message in response)
    {
        message.IsRead = true;  
    }

    // Persist the changes to the database
    await _context.SaveChangesAsync(cancellationToken);

    return response;
}

/// <summary>
/// Creates a new message in the database.
/// </summary>
/// <param name="message">The message to be created.</param>
/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
/// <returns>A task that represents the asynchronous operation.</returns>
public async Task Create(Message message, CancellationToken cancellationToken)
{
    /// Adds the message to the database asynchronously.
    await _context.Messages.AddAsync(message, cancellationToken);
    
    /// Saves the changes to the database asynchronously.
    await _context.SaveChangesAsync(cancellationToken);
}
}
