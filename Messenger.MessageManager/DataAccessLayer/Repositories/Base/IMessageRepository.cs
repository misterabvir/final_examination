using Messenger.MessageManager.DataAccessLayer.Models;
using Messenger.Shared.Abstractions.Repositories;

namespace Messenger.MessageManager.DataAccessLayer.Repositories.Base;

public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Message>> GetAndMarkAsReadByRecipient(Guid recipientId, CancellationToken cancellationToken);
    Task Create(Message message, CancellationToken cancellationToken);
}
