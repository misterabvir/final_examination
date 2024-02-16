using FluentAssertions;
using Messenger.MessageManager.BusinessLogicalLayer.Errors;
using Messenger.MessageManager.BusinessLogicalLayer.Services;
using Messenger.MessageManager.BusinessLogicalLayer.Services.Base;
using Messenger.MessageManager.DataAccessLayer.Models;
using Messenger.MessageManager.DataAccessLayer.Repositories.Base;
using Messenger.Shared.Abstractions.Results;
using Messenger.Shared.Contracts.Messages.Requests;
using Messenger.Shared.Contracts.Messages.Responses;
using Messenger.Shared.Contracts.Users.Requests;
using Messenger.Shared.Contracts.Users.Responses;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Messenger.MessageManager.Tests;

public class MessageServiceTests
{
    private readonly IHttpClientService _httpClientService;
    private readonly IMapperService _mapper;
    private readonly IMessageRepository _messageRepository;

    private readonly IMessageService _messageService;

    public MessageServiceTests()
    {
        _httpClientService = Substitute.For<IHttpClientService>();
        _mapper = Substitute.For<IMapperService>();
        _messageRepository = Substitute.For<IMessageRepository>();
        _messageService = new MessageService(_messageRepository, _httpClientService, _mapper);
    }

    [Fact]
    public async Task TestGetMessagesSuccessWithEmptyResponse()
    {
        //arrange
        var expectMessageRepositoryReceived = 1;
        var expectMapperReceived = 0;

        _messageRepository
            .GetAndMarkAsReadByRecipient(Arg.Any<Guid>(), default)
            .Returns(Enumerable.Empty<Message>());

        _mapper
            .Map<Message, MessageResponse>(Arg.Any<Message>())
            .Returns(new MessageResponse());

        //act
        var result = await _messageService.GetMessages(new UserIdentity(Guid.NewGuid(), string.Empty, string.Empty), default);

        //assert
        await _messageRepository.Received(expectMessageRepositoryReceived).GetAndMarkAsReadByRecipient(Arg.Any<Guid>(), default);
        _mapper.Received(expectMapperReceived).Map<Message, MessageResponse>(Arg.Any<Message>());
        result.Should().NotBeNull();
        result.Should().BeOfType<Result<IEnumerable<MessageResponse>, Error>>();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task TestGetMessagesSuccessWithNotEmptyResponse()
    {
        //arrange
        Message expectMessage = new()
        {
            RecipientId = Guid.NewGuid(),
            SenderId = Guid.NewGuid(),
            Text = "TestMessage"
        };
        var expectMessageRepositoryReceived = 1;
        var expectMapperReceived = 1;

        _messageRepository
            .GetAndMarkAsReadByRecipient(expectMessage.RecipientId, default)
            .Returns([expectMessage]);

        _mapper
            .Map<Message, MessageResponse>(Arg.Any<Message>())
            .Returns(new MessageResponse() { Text = expectMessage.Text, SenderId = expectMessage.SenderId });

        //act
        var result = await _messageService.GetMessages(new UserIdentity(expectMessage.RecipientId, string.Empty, string.Empty), default);

        //assert
        await _messageRepository.Received(expectMessageRepositoryReceived).GetAndMarkAsReadByRecipient(Arg.Any<Guid>(), default);
        _mapper.Received(expectMapperReceived).Map<Message, MessageResponse>(Arg.Any<Message>());

        result.Should().NotBeNull();
        result.Should().BeOfType<Result<IEnumerable<MessageResponse>, Error>>();

        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNullOrEmpty();
        
        result.Value!.FirstOrDefault().Should().NotBeNull();
        result.Value!.First().SenderId.Should().Be(expectMessage.SenderId);
        result.Value!.First().Text.Should().Be(expectMessage.Text);
    }

    [Fact]
    public async Task TestCreateMessageFailUserManagerUnavailable()
    {
        //arrange
        var expectText = "test message";
        var expectRecipientEmail = "email";
        var expectSenderId = Guid.NewGuid();
        MessageCreateRequest request = new MessageCreateRequest() { Text = expectText, RecipientEmail = expectRecipientEmail };
        UserIdentity user = new UserIdentity(expectSenderId, string.Empty, string.Empty);
        _httpClientService
            .Post<UserIsExistRequest?, UserExistingResponse>(Arg.Any<string>(), new UserIsExistRequest(expectRecipientEmail))
            .ReturnsNull();

        //act
        var result = await _messageService.CreateMessage(request, user, default);

        //assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Should().BeOfType<RequestToUserManagerFailed>();
    }

    [Fact]
    public async Task TestCreateMessageFailRecipientNotExist()
    {
        //arrange
        var expectText = "test message";
        var expectRecipientEmail = "email";
        var expectSenderId = Guid.NewGuid();
        var existRequest = new UserIsExistRequest(expectRecipientEmail);
        MessageCreateRequest request = new MessageCreateRequest() { Text = expectText, RecipientEmail = expectRecipientEmail }; 
        UserIdentity user = new UserIdentity(expectSenderId, string.Empty, string.Empty);
        _httpClientService
            .Post<UserIsExistRequest?, UserExistingResponse>(Arg.Any<string>(), existRequest)
            .Returns(new UserExistingResponse() { IsExisting = false });

        //act
        var result = await _messageService.CreateMessage(request, user, default);

        //assert
        await _httpClientService.Received(1).Post<UserIsExistRequest?, UserExistingResponse>(Arg.Any<string>(), existRequest);
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Should().BeOfType<RecipientUserNotExists>();
    }

    [Fact]
    public async Task TestCreateMessageSuccess()
    {
        //arrange
        var expectText = "test message";
        var expectRecipientEmail = "email";
        var expectRecipientId = Guid.NewGuid();
        var expectSenderId = Guid.NewGuid();
        var existRequest = new UserIsExistRequest(expectRecipientEmail);
        MessageCreateRequest request = new MessageCreateRequest() { Text = expectText, RecipientEmail = expectRecipientEmail };
        UserIdentity user = new UserIdentity(expectSenderId, string.Empty, string.Empty);
        _httpClientService
            .Post<UserIsExistRequest?, UserExistingResponse>(Arg.Any<string>(), existRequest)
            .Returns(new UserExistingResponse() { IsExisting = true, UserId = expectRecipientId });

        //act
        var result = await _messageService.CreateMessage(request, user, default);

        //assert
        await _httpClientService.Received(1).Post<UserIsExistRequest?, UserExistingResponse>(Arg.Any<string>(), existRequest);
        await _messageRepository.Received(1).Create(Arg.Any<Message>(), Arg.Any<CancellationToken>());
        
        result.Should().NotBeNull();
        
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<MessageCreateResponse>();
        
        result.Value!.IsSuccess.Should().BeTrue();
    }
}