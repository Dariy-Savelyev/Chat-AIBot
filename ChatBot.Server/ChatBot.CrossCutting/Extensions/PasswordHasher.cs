using System.Security.Cryptography;
using System.Text;

namespace ChatBot.CrossCutting.Extensions;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashedBytes = SHA256.HashData(passwordBytes);

        return Convert.ToBase64String(hashedBytes);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var hashedInput = HashPassword(password);

        return hashedInput.Equals(hashedPassword);
    }
}