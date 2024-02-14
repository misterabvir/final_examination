using Messenger.UserManager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.UserManager.DataAccessLayer.Configurations;

/// <summary>
/// Configures the entity of type User.
/// </summary>
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the User entity for the database.
    /// </summary>
    /// <param name="builder">The entity type builder for User.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Set the table name for the User
        builder.ToTable("users");

        // Configure the primary key and its name
        builder.HasKey(x => x.Id).HasName("users_pkey");

        // Configure the column name for the Id property
        builder.Property(x => x.Id).HasColumnName("user_id");

        // Configure the column name and length for the Email property
        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(64);

        // Configure the column name for the Password property
        builder.Property(x => x.Password).HasColumnName("password");

        // Configure the column name for the Salt property
        builder.Property(x => x.Salt).HasColumnName("salt");

        // Configure the column name for the RoleId property
        builder.Property(x => x.RoleId).HasColumnName("role_id");

        // Configure the foreign key relationship with RoleType
        builder.HasOne(x => x.Role)
               .WithMany()
               .HasForeignKey(x => x.RoleId)
               .HasConstraintName("users_role_id_fkey");
    }
}
