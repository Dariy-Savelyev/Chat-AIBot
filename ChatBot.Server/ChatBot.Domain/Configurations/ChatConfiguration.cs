using ChatBot.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatBot.Domain.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(100);

        builder.Property(p => p.DateCreate);

        builder.HasOne(c => c.Creator)
            .WithMany(u => u.CreatedChats)
            .HasForeignKey(c => c.CreatorId);
    }
}