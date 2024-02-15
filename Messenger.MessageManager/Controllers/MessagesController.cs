using Messenger.MessageManager.BusinessLogicalLayer.Services.Base;
using Messenger.Shared.Abstractions.Controllers;
using Messenger.Shared.Contracts.Messages.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.MessageManager.Controllers;

[Route("messages")]

public class MessagesController : BaseController
{
    private readonly IMessageService _messageService;

    /// <summary>
    /// Constructor for MessagesController
    /// </summary>
    /// <param name="messageService">The message service</param>
    /// <param name="logger">The logger</param>
    public MessagesController(IMessageService messageService, ILogger<MessagesController> logger) : base(logger)
    {
        _messageService = messageService;
    }

    // Ensures that the user is authorized to access the endpoint
    [Authorize]
    [HttpGet(template: "get-messages")]
    // Retrieves messages for the current user
    public async Task<IActionResult> GetMessages(CancellationToken cancellationToken)
    {
        // Retrieves the current user
        var user = GetCurrentUser();

        // Returns a 403 Forbidden status if the user is not authenticated
        if (user is null)
        {
            return Forbid();
        }

        // Retrieves messages for the current user from the message service
        var response = await _messageService.GetMessages(user, cancellationToken);

        // Returns the messages if the retrieval is successful
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        // Returns a problem result if the retrieval is unsuccessful
        return ProblemActionResult(response.Error!);
    }

    /// <summary>
    /// Endpoint for sending a message
    /// </summary>
    [Authorize]
    [HttpPost(template: "send-message")]
    public async Task<IActionResult> SendMessage([FromBody] MessageCreateRequest request)
    {
        // Get the current user
        var user = GetCurrentUser();

        // If user is not authenticated, forbid the request
        if (user is null)
        {
            return Forbid();
        }

        // Create the message and return the response
        var response = await _messageService.CreateMessage(request, user, CancellationToken.None);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }
        return ProblemActionResult(response.Error!);
    }
}
