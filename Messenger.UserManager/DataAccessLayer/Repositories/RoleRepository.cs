using Messenger.Shared.Enums;
using Messenger.UserManager.DataAccessLayer.Contexts;
using Messenger.UserManager.DataAccessLayer.Repositories.Base;
using Messenger.UserManager.Domain;
using Microsoft.EntityFrameworkCore;

namespace Messenger.UserManager.DataAccessLayer.Repositories;

internal sealed class RoleRepository : IRoleRepository
{
    private readonly UserManagerContext _context;

    /// <summary>
    /// Constructor for RoleRepository class
    /// </summary>
    /// <param name="context">The user manager context</param>
    public RoleRepository(UserManagerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a role based on the role type asynchronously.
    /// </summary>
    /// <param name="roleType">The type of role to retrieve</param>
    /// <param name="cancellationToken">A CancellationToken to cancel the asynchronous operation.</param>
    /// <returns>The role with the specified role type, or null if not found.</returns>

    public async Task<Role?> GetRoleByRoleTypeAsync(RoleType roleType, CancellationToken cancellationToken = default)
    {
        return await _context.Roles.FirstOrDefaultAsync(role => role.RoleType == roleType, cancellationToken);
    }
}