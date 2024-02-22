using Messenger.MessageManager.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.MessageManager.DataAccessLayer.Configurations;

public class AvailableUserEntityTypeConfiguration : IEntityTypeConfiguration<AvailableUser>
{
    /// <summary>
    /// Configures the AvailableUser entity.
    /// </summary>
    /// <param name="builder">The entity type builder for AvailableUser.</param>
    public void Configure(EntityTypeBuilder<AvailableUser> builder)
    {
        // Set table name
        builder.ToTable("users");

        // Set primary key and column name
        builder.HasKey(x => x.Id).HasName("users_pk");
        builder.Property(x => x.Id).HasColumnName("user_id");

        // Set column email for text property
        builder.Property(x => x.Email).HasColumnName("email");
    }
}