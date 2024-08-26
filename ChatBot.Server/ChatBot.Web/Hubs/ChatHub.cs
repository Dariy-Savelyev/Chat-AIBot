using ChatBot.Application.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Web.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(HubMessageModel model)
    {
        await Clients.All.SendAsync("ReceiveMessage", model);
    }
}