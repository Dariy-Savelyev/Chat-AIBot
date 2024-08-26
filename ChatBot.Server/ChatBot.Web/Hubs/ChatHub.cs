using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Web.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(HubMessageModel model, IMessageService service, IMapper mapper)
    {
        var message = mapper.Map<MessageModel>(model);
        
        var messageId = await service.SendMessageAsync(message, model.UserId);
        model.Id = messageId;

        await Clients.All.SendAsync("ReceiveMessage", model);
    }
}