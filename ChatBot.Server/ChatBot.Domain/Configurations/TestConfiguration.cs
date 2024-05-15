using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ChatBot.Domain.Models;

namespace ChatBot.Domain.Configurations;

public class TestConfiguration : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Field).HasMaxLength(100);
    }
}