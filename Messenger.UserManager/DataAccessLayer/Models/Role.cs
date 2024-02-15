using Messenger.Shared.Abstractions.Models;
using Messenger.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.UserManager.Models;

/// <summary> Entity for user RoleType </summary>
[Table("roles")]
public sealed class Role : Entity
{
    /// <summary> Type of user role </summary>
    [Column("user_role")]
    public required RoleType RoleType { get; set; } = RoleType.User;

    public override string ToString() => RoleType.ToString();
}
