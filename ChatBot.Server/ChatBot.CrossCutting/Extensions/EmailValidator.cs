using System.Text.RegularExpressions;

namespace ChatBot.CrossCutting.Extensions;

public static class EmailValidator
{
    public static bool IsValidEmail(string email)
    {
        var pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";

        if (Regex.IsMatch(email, pattern, RegexOptions.NonBacktracking, TimeSpan.FromMilliseconds(100)))
        {
            return true;
        }

        return false;
    }
}