using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

public class MessageController(IMessageService service) : BaseController
{
    [HttpPost]
    public async Task<int> Send(MessageModel model)
    {
        return await service.SendMessageAsync(model, User.GetUserId());
    }

    [HttpPost]
    public async Task SetEmote(MessageEmoteModel model)
    {
        await service.SetEmoteAsync(model);
    }

    [HttpGet]
    public async Task<IEnumerable<GetAllMessageModel>> GetAllMessages(int chatId)
    {
        return await service.GetAllMessagesAsync(chatId);
    }
}