﻿using ChatBot.Domain.Models.Base;

namespace ChatBot.Domain.Models;

public class User : BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public ICollection<Chat> CreatedChats { get; set; }
    public ICollection<Chat> Chats { get; set; }
}