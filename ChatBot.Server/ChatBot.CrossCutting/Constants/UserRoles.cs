namespace ChatBot.CrossCutting.Constants;

public static class UserRoles
{
    public const string Administrator = nameof(Administrator);
    public const string User = nameof(User);

    public static readonly IReadOnlyCollection<string> UserRoleList =
    [
        Administrator,
        User,
    ];
}