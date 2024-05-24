using System.Net.Mail;

namespace ChatBot.CrossCutting.Extensions;

public static class EmailValidator
{
    public static bool IsValidEmail(string email)
    {
        return MailAddress.TryCreate(email, out _);
    }
}