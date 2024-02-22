using Messenger.MessageManager.DataAccessLayer.Models;
using Messenger.Shared.Abstractions.Repositories;

namespace Messenger.MessageManager.DataAccessLayer.Repositories.Base;

public interface IUserRepository : IRepository<AvailableUser>
{
    public Task Create(AvailableUser user, CancellationToken cancellationToken);
    public Task<AvailableUser?> GetByEmail(string email, CancellationToken cancellationToken);
    public Task<AvailableUser?> GetById(Guid id, CancellationToken cancellationToken);
    public Task Delete(Guid id, CancellationToken cancellationToken);   
}
