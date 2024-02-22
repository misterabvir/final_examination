using MassTransit;
using Messenger.MessageManager.DataAccessLayer.Repositories.Base;
using Messenger.Shared.Contracts.Events;

namespace Messenger.MessageManager.BusinessLogicalLayer.Consumes;

public class UserDeletedConsumer : IConsumer<UserDeletedEvent>
{
    private readonly IUserRepository _userRepository;
    public UserDeletedConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserDeletedEvent> context)
    {
        var eventUserDeleted = context.Message;

        await _userRepository.Delete(eventUserDeleted.Id, default);
    }
}