using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

public class MessageController(IMessageService service) : BaseController
{
    [HttpPost]
    public async Task Send(MessageModel model)
    {
        await service.SendMessageAsync(model, User.GetUserId());
    }

    [HttpGet]
    public async Task<IEnumerable<GetAllMessageModel>> GetAllMessages(int chatId)
    {
        return await service.GetAllMessagesAsync(chatId);
    }
}