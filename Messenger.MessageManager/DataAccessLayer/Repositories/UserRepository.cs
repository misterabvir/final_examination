using Messenger.MessageManager.DataAccessLayer.Contexts;
using Messenger.MessageManager.DataAccessLayer.Models;
using Messenger.MessageManager.DataAccessLayer.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.MessageManager.DataAccessLayer.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MessageManagerContext _context;

    public UserRepository(MessageManagerContext context)
    {
        _context = context;
    }

    public async Task Create(AvailableUser user, CancellationToken cancellationToken)
    {
        await _context.AvailableUsers.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        _context.AvailableUsers.Remove((await GetById(id, cancellationToken))!);
        await Task.CompletedTask;
    }

    public async Task<AvailableUser?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return await _context.AvailableUsers.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<AvailableUser?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _context.AvailableUsers.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}
