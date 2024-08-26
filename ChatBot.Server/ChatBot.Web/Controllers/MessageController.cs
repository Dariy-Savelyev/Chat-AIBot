using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using ChatBot.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Web.Controllers;

public class MessageController(IMessageService service) : BaseController
{
    [HttpPost]
    public async Task<int> Send(MessageModel model, IHubContext<ChatHub> hubContext)
    {
        var messageId = await service.SendMessageAsync(model, User.GetUserId());

        object message = new
        {
            Id = messageId,
            Content = model.Content,
            ChatId = model.ChatId,
            UserId = User.GetUserId()
        };

        await hubContext.Clients.All.SendAsync("ReceiveMessage", message);

        return messageId;
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