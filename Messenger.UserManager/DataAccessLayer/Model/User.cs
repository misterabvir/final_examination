using Messenger.Shared.Abstractions.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messenger.UserManager.Domain
{
    /// <summary>
    /// Entity for user
    /// </summary> 
    [Table("users")]
    public sealed class User : Entity
    {
        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        [Column("email")]
        [Required]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user stored as a byte array.
        /// </summary>
        [Column("password")]
        [Required]
        public required byte[] Password { get; set; }

        /// <summary>
        /// Gets or sets the salt of the user's password stored as a byte array.
        /// </summary>
        [Column("salt")]
        [Required]
        public required byte[] Salt { get; set; }

        /// <summary>
        /// Gets or sets the role ID of the user.
        /// </summary>
        [Column("role_id")]
        [Required]
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role entity related to the user.
        /// </summary>
        public Role? Role { get; set; }
    }
}