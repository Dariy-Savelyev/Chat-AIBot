using ChatBot.Domain.Models.Base;

namespace ChatBot.Domain.Models;

public class Chat : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateCreate { get; set; }
    public string CreatorId { get; set; } = string.Empty;
    public User Creator { get; set; }
    public ICollection<User> Users { get; set; } = [];
}