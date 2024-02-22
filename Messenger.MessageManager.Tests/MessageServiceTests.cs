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
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    private readonly IMessageService _messageService;

    public MessageServiceTests()
    {

        _messageRepository = Substitute.For<IMessageRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _messageService = new MessageService(_messageRepository, _userRepository);
    }

    [Fact]
    public async Task TestGetMessagesSuccessWithEmptyResponse()
    {
        //arrange
        var expectMessageRepositoryReceived = 1;

        _messageRepository
            .GetAndMarkAsReadByRecipient(Arg.Any<Guid>(), default)
            .Returns(Enumerable.Empty<Message>());
        //act
        var result = await _messageService.GetMessages(new UserIdentity(Guid.NewGuid(), string.Empty, string.Empty), default);

        //assert
        await _messageRepository.Received(expectMessageRepositoryReceived).GetAndMarkAsReadByRecipient(Arg.Any<Guid>(), default);

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
        var expectedEmail = "email";


        _messageRepository
            .GetAndMarkAsReadByRecipient(expectMessage.RecipientId, default)
            .Returns([expectMessage]);
        _userRepository.GetById(expectMessage.SenderId, default).Returns(new AvailableUser() { Email = expectedEmail, Id = expectMessage.RecipientId});

        //act
        var result = await _messageService.GetMessages(new UserIdentity(expectMessage.RecipientId, string.Empty, string.Empty), default);

        //assert
        await _messageRepository.Received(expectMessageRepositoryReceived).GetAndMarkAsReadByRecipient(Arg.Any<Guid>(), default);

        result.Should().NotBeNull();
        result.Should().BeOfType<Result<IEnumerable<MessageResponse>, Error>>();

        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeNullOrEmpty();
        result.Value!.FirstOrDefault().Should().NotBeNull();
        result.Value!.First().Sender.Should().Be(expectedEmail);
        result.Value!.First().Text.Should().Be(expectMessage.Text);
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
        _userRepository.GetByEmail(expectRecipientEmail, default).ReturnsNull();

        //act
        var result = await _messageService.CreateMessage(request, user, default);

        //assert
        await _userRepository.Received(1).GetByEmail(Arg.Any<string>(), default);
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
        _userRepository.GetByEmail(expectRecipientEmail, default).Returns(new AvailableUser() { Email = expectRecipientEmail, Id = expectRecipientId });

        //act
        var result = await _messageService.CreateMessage(request, user, default);

        //assert
        await _userRepository.Received(1).GetByEmail(expectRecipientEmail, default);
        await _messageRepository.Received(1).Create(Arg.Any<Message>(), Arg.Any<CancellationToken>());
        
        result.Should().NotBeNull();
        
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeNull();
        result.Value.Should().BeOfType<MessageCreateResponse>();
        
        result.Value!.IsSuccess.Should().BeTrue();
    }
}