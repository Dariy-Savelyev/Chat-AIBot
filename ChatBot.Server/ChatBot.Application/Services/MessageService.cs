﻿using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.Domain.Models;
using ChatBot.Domain.RepositoryInterfaces;

namespace ChatBot.Application.Services;

public class MessageService(IMessageRepository messageRepository, IMapper mapper) : IMessageService
{
    public async Task<int> SendMessageAsync(MessageModel model, string userId)
    {
        var message = mapper.Map<Message>(model);
        message.UserId = userId;

        await messageRepository.AddAsync(message);

        return message.Id;
    }

    public async Task<IEnumerable<GetAllMessageModel>> GetAllMessagesAsync(int chatId)
    {
        var dataBaseMessages = await messageRepository.GetAllAsync();

        var chatMessages = dataBaseMessages.Where(x => x.ChatId == chatId);

        var messages = mapper.Map<IEnumerable<GetAllMessageModel>>(chatMessages);

        var listOfMessages = new List<GetAllMessageModel>();
        listOfMessages.AddRange(messages);

        return listOfMessages;
    }

    public async Task SetEmoteAsync(MessageEmoteModel model)
    {
        var emote = mapper.Map<Message>(model);

        await messageRepository.AddEmoteAsync(emote.Id, emote.Emote);
    }
}