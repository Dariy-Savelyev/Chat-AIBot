using System.ComponentModel.DataAnnotations;

namespace ChatBot.Domain.Models.Base;

public interface IBaseDomainModel<TId>
{
    [Key]
    TId Id { get; set; }
}