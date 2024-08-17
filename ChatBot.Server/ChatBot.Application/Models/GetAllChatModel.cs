﻿namespace ChatBot.Application.Models;

public class GetAllChatModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Joined { get; set; }
    public ICollection<string> UserIds { get; set; } = [];
}