using ChatBot.Application.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatBot.Web.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(MessageModel message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}