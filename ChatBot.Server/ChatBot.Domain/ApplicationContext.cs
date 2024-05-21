using ChatBot.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Domain;

public sealed class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> builder) : base(builder)
    {
    }

    public DbSet<Test> Tests { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings:AppConnectionString"));
        base.OnConfiguring(optionsBuilder);
    }
}