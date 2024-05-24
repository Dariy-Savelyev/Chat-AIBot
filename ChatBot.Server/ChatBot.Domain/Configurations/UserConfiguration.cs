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
    }
}