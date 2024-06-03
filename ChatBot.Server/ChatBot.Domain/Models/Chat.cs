using ChatBot.Domain.Models.Base;

namespace ChatBot.Domain.Models;

public class Chat : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateCreate { get; set; }
    public int CreatorId { get; set; }
    public User Creator { get; set; }
    public ICollection<User> Users { get; set; }
}