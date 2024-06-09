using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

public class ChatController(IChatService service) : BaseController
{
    [HttpPost]
    public async Task Create(ChatModel model)
    {
        await service.CreateChatAsync(model, User.GetUserId());
    }

    [HttpPost]
    public async Task Join(JoinToChatModel model)
    {
        await service.JoinChatAsync(User.GetUserId(), model.ChatId);
    }
}