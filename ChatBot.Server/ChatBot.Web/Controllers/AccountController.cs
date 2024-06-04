using ChatBot.Application.Models;
using ChatBot.Application.Models.Tokens;
using ChatBot.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

[AllowAnonymous]
public class AccountController(IAccountService service) : BaseController
{
    [HttpPost]
    public async Task Registration(RegistrationModel model)
    {
        await service.RegistrationAsync(model);
    }

    [HttpPost]
    public async Task<RefreshTokenModel> Login(LoginModel model)
    {
        return await service.LoginAsync(model);
    }
}