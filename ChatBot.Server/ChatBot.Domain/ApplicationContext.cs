using ChatBot.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ChatBot.Domain;

public sealed class ApplicationContext : IdentityDbContext<User>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> builder)
        : base(builder)
    {
    }

    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Seed();
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey, x.UserId });
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.RoleId, x.UserId });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.LoginProvider, x.UserId, x.Name });
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}