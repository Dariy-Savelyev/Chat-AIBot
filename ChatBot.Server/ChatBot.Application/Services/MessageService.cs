using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Application.Services;

public class MessageService(IMessageRepository messageRepository, IMapper mapper) : IMessageService
{
    public async Task SendMessageAsync(MessageModel model, string userId)
    {
        var message = mapper.Map<Message>(model);
        message.UserId = userId;

        await messageRepository.AddAsync(message);
    }

    public async Task<IEnumerable<GetAllMessageModel>> GetAllMessagesAsync(int chatId)
    {
        var dataBaseMessages = await messageRepository.GetAllAsync();

        var messages = mapper.Map<IEnumerable<GetAllMessageModel>>(dataBaseMessages.Where(x => x.ChatId == chatId));

        var listOfMessages = new List<GetAllMessageModel>();
        listOfMessages.AddRange(messages);

        return listOfMessages;
    }
}