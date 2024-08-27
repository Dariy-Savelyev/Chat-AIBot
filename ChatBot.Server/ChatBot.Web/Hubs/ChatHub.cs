using AutoMapper;
using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Web.Hubs;

public class ChatHub : Hub
{
    public async Task AddMessage(HubAddMessageModel model, IMessageService service, IMapper mapper)
    {
        var userId = Context.User!.GetUserId();

        var messageId = await service.SendMessageAsync(model, userId);

        var message = mapper.Map<HubMessageModel>(model);
        message.Id = messageId;
        message.UserId = userId;

        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}