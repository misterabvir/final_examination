
using Messenger.Shared.Abstractions.Repositories;
using Messenger.Shared.Enums;
using Messenger.UserManager.Domain;

namespace Messenger.UserManager.DataAccessLayer.Repositories.Base;

/// <summary>
/// Interface for interacting with role entities in the repository.
/// </summary>
public interface IRoleRepository : IRepository<Role>
{
    /// <summary>
    /// Retrieves a role entity from the repository based on its role type.
    /// </summary>
    /// <param name="roleType">The type of role to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning a role entity if found. Returns null if no role matches the specified role type.</returns>
    Task<Role?> GetRoleByRoleTypeAsync(RoleType roleType, CancellationToken cancellationToken = default);
}

