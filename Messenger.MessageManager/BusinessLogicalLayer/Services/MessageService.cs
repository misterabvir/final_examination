using Messenger.MessageManager.BusinessLogicalLayer.Errors;
using Messenger.MessageManager.BusinessLogicalLayer.Services.Base;
using Messenger.MessageManager.DataAccessLayer.Models;
using Messenger.MessageManager.DataAccessLayer.Repositories.Base;
using Messenger.Shared.Abstractions.Results;
using Messenger.Shared.Contracts.Messages.Requests;
using Messenger.Shared.Contracts.Messages.Responses;
using Messenger.Shared.Contracts.Users.Responses;

namespace Messenger.MessageManager.BusinessLogicalLayer.Services;

public class MessageService : IMessageService
{
    private readonly IHttpClientService _httpClientService;
    private readonly IMapperService _mapper;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Constructor for MessageService class
    /// </summary>
    /// <param name="messageRepository">The message repository</param>
    /// <param name="httpClientService">The HTTP httpClientService</param>
    /// <param name="mapper">The mapper</param>
    public MessageService(IMessageRepository messageRepository, IHttpClientService httpClientService, IMapperService mapper, IUserRepository userRepository)
    {
        _httpClientService = httpClientService;
        _messageRepository = messageRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Retrieves messages for a specific user.
    /// </summary>
    /// <param name="user">The user identity for whom the messages are retrieved.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of message responses.</returns>
    public async Task<Result<IEnumerable<MessageResponse>, Error>> GetMessages(UserIdentity user, CancellationToken cancellation)
    {
        var messages = await _messageRepository.GetAndMarkAsReadByRecipient(user.Id, cancellation);
        var response = messages.Select(m => new MessageResponse()
        {
            Sender = _userRepository.GetById(m.SenderId, default).Result!.Email,
            Text = m.Text
        }).ToList();
        return response;
    }

    /// <summary>
    /// Creates a message for the specified recipient.
    /// </summary>
    /// <param name="request">The message creation request.</param>
    /// <param name="user">The user creating the message.</param>
    /// <param name="cancellation">Cancellation token.</param>
    /// <returns>Result of the message creation operation.</returns>
    public async Task<Result<MessageCreateResponse, Error>> CreateMessage(MessageCreateRequest request, UserIdentity user, CancellationToken cancellation)
    {
        // Check if the user exists
        var recipient = await _userRepository.GetByEmail(request.RecipientEmail, cancellation);
        if (recipient == null)
        {
            return new RecipientUserNotExists();
        }

        // Create the message
        var message = new Message
        {
            RecipientId = recipient.Id,
            SenderId = user.Id,
            Text = request.Text
        };
        await _messageRepository.Create(message, cancellation);
        return new MessageCreateResponse(true);
    }
}
