using MassTransit;
using Messenger.MessageManager.DataAccessLayer.Contexts;
using Messenger.MessageManager.DataAccessLayer.Models;
using Messenger.MessageManager.DataAccessLayer.Repositories.Base;
using Messenger.Shared.Contracts.Events;

namespace Messenger.MessageManager.BusinessLogicalLayer.Consumes;

public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly IUserRepository _userRepository;
    public UserRegisteredConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var eventUserRegistered = context.Message;
        var user = new AvailableUser() { Id =  eventUserRegistered.Id, Email = eventUserRegistered.Email };
        await _userRepository.Create(user, default);
    }
}
