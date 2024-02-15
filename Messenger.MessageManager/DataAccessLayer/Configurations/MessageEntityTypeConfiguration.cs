using Messenger.MessageManager.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.MessageManager.DataAccessLayer.Configurations;

public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
{
/// <summary>
/// Configures the Message entity.
/// </summary>
/// <param name="builder">The entity type builder for Message.</param>
public void Configure(EntityTypeBuilder<Message> builder)
{
    // Set table name
    builder.ToTable("messages");

    // Set primary key and column name
    builder.HasKey(x => x.Id).HasName("messages_pk");
    builder.Property(x => x.Id).HasColumnName("message_id");

    // Set column name for text property
    builder.Property(x => x.Text).HasColumnName("text");
    
    // Set index and column name for SenderId
    builder.HasIndex(x=>x.SenderId);
    builder.Property(x=>x.SenderId).HasColumnName("sender_id");

    // Set index and column name for RecipientId
    builder.HasIndex(x => x.RecipientId);
    builder.Property(x=>x.RecipientId).HasColumnName("recipient_id");

    // Set column name for IsRead property
    builder.Property(x => x.IsRead).HasColumnName("is_read");
}
}
