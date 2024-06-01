using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ChatController(IChatService service) : ControllerBase
{
    [HttpPost]
    public async Task Create(ChatModel model)
    {
        await service.CreateChatAsync(model);
    }
}