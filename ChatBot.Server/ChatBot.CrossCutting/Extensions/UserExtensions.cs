using System.Security.Claims;

namespace ChatBot.CrossCutting.Extensions;

public static class UserExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.GetValue(ClaimTypes.NameIdentifier);
    }

    private static string GetValue(this ClaimsPrincipal user, string key)
    {
        return user.Claims.SingleOrDefault(x => x.Type == key)?.Value ?? string.Empty;
    }
}