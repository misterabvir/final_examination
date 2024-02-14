using Messenger.UserManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.UserManager.DataAccessLayer.Configurations;

/// <summary>
/// Configures the entity for the RoleType class
/// </summary>
public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    /// <summary>
    /// Configures the entity for the RoleType class
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Set the table name to "roles"
        builder.ToTable("roles");

        // Set the primary key and its name
        builder.HasKey(x => x.Id).HasName("roles_pkey");

        // Set the column name for the Id property
        builder.Property(x => x.Id).HasColumnName("role_id");

        // Set the column name for the RoleType property
        builder.Property(x => x.RoleType).HasColumnName("role");
    }
}
