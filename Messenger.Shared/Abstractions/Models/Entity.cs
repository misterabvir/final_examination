using System.ComponentModel.DataAnnotations;

namespace Messenger.Shared.Abstractions.Models
{
    /// <summary>
    /// Abstract Entity for all entities
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the entity.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
