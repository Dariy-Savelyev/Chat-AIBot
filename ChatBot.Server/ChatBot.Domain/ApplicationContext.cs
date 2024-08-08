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
    public DbSet<Message> Messages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey, x.UserId });
        builder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.RoleId, x.UserId });
        builder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.LoginProvider, x.UserId, x.Name });
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}