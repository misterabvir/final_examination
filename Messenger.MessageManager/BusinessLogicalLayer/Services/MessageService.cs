using AutoMapper;
using Messenger.MessageManager.BusinessLogicalLayer.Errors;
using Messenger.MessageManager.BusinessLogicalLayer.Services.Base;
using Messenger.MessageManager.DataAccessLayer.Models;
using Messenger.MessageManager.DataAccessLayer.Repositories.Base;
using Messenger.Shared.Abstractions.Results;
using Messenger.Shared.Contracts.Messages.Requests;
using Messenger.Shared.Contracts.Messages.Responses;
using Messenger.Shared.Contracts.Users.Requests;
using Messenger.Shared.Contracts.Users.Responses;
using System.Text.Json;

namespace Messenger.MessageManager.BusinessLogicalLayer.Services;

public class MessageService : IMessageService
{
    private readonly HttpClient _client;
    private readonly IMapper _mapper;
    private readonly IMessageRepository _messageRepository;

    /// <summary>
    /// Constructor for MessageService class
    /// </summary>
    /// <param name="messageRepository">The message repository</param>
    /// <param name="client">The HTTP client</param>
    /// <param name="logger">The logger</param>
    /// <param name="mapper">The mapper</param>
    public MessageService(IMessageRepository messageRepository, HttpClient client, ILogger<MessageService> logger, IMapper mapper)
    {
        _client = client;
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves messages for a specific user.
    /// </summary>
    /// <param name="user">The user identity for whom the messages are retrieved.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of message responses.</returns>
    public async Task<Result<IEnumerable<MessageResponse>, Error>> GetMessages(UserIdentity user, CancellationToken cancellation)
    {
        // Retrieve messages from the repository based on the user's identity
        var messages = await _messageRepository.GetAndMarkAsReadByRecipient(user.Id, cancellation);

        // Map the retrieved messages to message responses using the mapper
        var response = messages.Select(_mapper.Map<MessageResponse>)!.ToList();

        // Return the mapped message responses
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
        var existRequest = await Post<UserIsExistRequest?, UserExistingResponse>(
            url: "http://ocelot_manager:8080/users/is-user-exist",
            request: new UserIsExistRequest(user.Id));
        if (existRequest is null)
        {
            return new RequestToUserManagerFailed();
        }
        if (!existRequest.IsExist)
        {
            return new RecipientUserNotExists();
        }

        // Create the message
        var message = new Message
        {
            RecipientId = request.RecipientId,
            SenderId = user.Id,
            Text = request.Text
        };
        await _messageRepository.Create(message, cancellation);
        return new MessageCreateResponse(true);
    }

    /// <summary>
    /// Posts data to the specified URL and returns the deserialized response.
    /// </summary>
    /// <typeparam name="T">The type of the request data</typeparam>
    /// <typeparam name="E">The type of the response data</typeparam>
    /// <param name="url">The URL to post the data to</param>
    /// <param name="request">The data to be posted</param>
    /// <returns>The deserialized response data</returns>
    public async Task<E?> Post<T, E>(string url, T request)
    {
        var response = await _client.PostAsJsonAsync(url, request);
        return await Response<E>(response);
    }

    /// <summary>
    /// Parses the content of the HTTP response message and deserializes it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the content to.</typeparam>
    /// <param name="response">The HTTP response message to parse and deserialize.</param>
    /// <returns>The deserialized content of the HTTP response message.</returns>
    private static async Task<T?> Response<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(json);
        return result;
    }
}
