using Messenger.Shared.Abstractions.Results;
using Messenger.Shared.Contracts.Messages.Requests;
using Messenger.Shared.Contracts.Messages.Responses;
using Messenger.Shared.Contracts.Users.Responses;

namespace Messenger.MessageManager.BusinessLogicalLayer.Services.Base;

public interface IMessageService
{
    Task<Result<IEnumerable<MessageResponse>, Error>> GetMessages(UserIdentity user, CancellationToken cancellation);
    Task<Result<MessageCreateResponse, Error>> CreateMessage(MessageCreateRequest request, UserIdentity user, CancellationToken cancellation);
}
