using ChatBot.Domain.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace ChatBot.Domain.Models;

public class User : IdentityUser, IBaseDomainModel<string>
{
    public ICollection<Chat> CreatedChats { get; set; } = [];
    public ICollection<Chat> Chats { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}