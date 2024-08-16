﻿using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

public class ChatController(IChatService service) : BaseController
{
    [HttpPost]
    public async Task<int> Create(ChatModel model)
    {
        return await service.CreateChatAsync(model, User.GetUserId());
    }

    [HttpPost]
    public async Task Join(JoinToChatModel model)
    {
        await service.JoinChatAsync(model, User.GetUserId());
    }

    [HttpGet]
    public async Task<bool> IsUserJoined(int chatId)
    {
        return await service.IsUserInChatAsync(chatId, User.GetUserId());
    }

    [HttpGet]
    public async Task<IEnumerable<GetAllChatModel>> GetAllChats()
    {
        return await service.GetAllChatsAsync(User.GetUserId());
    }

    [HttpGet]
    public async Task<string> GetCurrentUserId()
    {
        return await Task.Run(() => User.GetUserId());
    }
}