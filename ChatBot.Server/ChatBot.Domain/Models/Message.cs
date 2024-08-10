using ChatBot.Domain.Models.Base;

namespace ChatBot.Domain.Models;

public class Message : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public DateTime SendDate { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ChatId { get; set; }
    public bool? Emote { get; set; }

    public Chat Chat { get; set; }
    public User User { get; set; }
}