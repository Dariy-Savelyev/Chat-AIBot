using ChatBot.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatBot.Domain.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Content);

        builder.Property(p => p.UserId).HasMaxLength(450);

        builder.Property(p => p.ChatId);

        builder.Property(p => p.SendDate);

        builder.Property(p => p.Emote);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Chat)
            .WithMany(u => u.Messages)
            .HasForeignKey(c => c.ChatId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}