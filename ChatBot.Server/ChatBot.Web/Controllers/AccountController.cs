using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpPost]
    public async Task<bool> Registration(RegistrationModel model)
    {
        return await service.RegistrationAsync(model);
    }

    [HttpPost]
    public async Task<bool> Login(LoginModel model)
    {
        return await service.LoginAsync(model);
    }
}