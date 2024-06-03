using ChatBot.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatBot.Domain.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.UserName).HasMaxLength(100);

        builder.Property(p => p.Email).HasMaxLength(100);

        builder.Property(p => p.PasswordHash).HasMaxLength(100);

        builder.HasIndex(x => new { x.UserName }).IsUnique();

        builder.HasIndex(x => new { x.Email }).IsUnique();

        builder.HasMany(u => u.Chats)
            .WithMany(c => c.Users)
            .UsingEntity<Dictionary<string, object>>(
                "UserChat",
                j => j.HasOne<Chat>().WithMany().HasForeignKey("ChatId").OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict),
                j => j.HasKey("ChatId", "UserId"));

        builder.HasMany(u => u.CreatedChats)
            .WithOne(c => c.Creator)
            .HasForeignKey(c => c.CreatorId);
    }
}