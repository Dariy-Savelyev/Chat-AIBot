using ChatBot.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Domain;

public sealed class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> builder)
        : base(builder)
    {
    }

    public DbSet<Test> Tests { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}