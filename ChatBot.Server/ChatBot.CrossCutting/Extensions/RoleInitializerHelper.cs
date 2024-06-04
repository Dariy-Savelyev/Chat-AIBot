using ChatBot.CrossCutting.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.CrossCutting.Extensions;

public static class RoleInitializerHelper
{
    public static void SeedRoles(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        SeedRoles(roleManager!);
    }

    private static void SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in UserRoles.UserRoleList)
        {
            CreateRoleAsync(roleManager, role).GetAwaiter().GetResult();
        }
    }

    private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}