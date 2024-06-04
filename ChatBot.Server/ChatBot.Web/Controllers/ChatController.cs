using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

public class ChatController(IChatService service) : BaseController
{
    [HttpPost]
    public async Task Create(ChatModel model)
    {
        await service.CreateChatAsync(model, User.GetUserId());
    }
}