using System.Text.RegularExpressions;

namespace ChatBot.CrossCutting.Extensions;

public static class EmailValidator
{
    public static bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        string incompletePattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+$";

        if (Regex.IsMatch(email, pattern))
        {
            return true;
        }
        else if (Regex.IsMatch(email, incompletePattern))
        {
            return true;
        }

        return false;
    }
}