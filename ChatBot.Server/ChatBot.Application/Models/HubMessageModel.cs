namespace ChatBot.Application.Models;

public class HubMessageModel
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int ChatId { get; set; }
}