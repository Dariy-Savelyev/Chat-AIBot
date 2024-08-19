using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

public class AccountController(IAccountService service) : BaseController
{
    [AllowAnonymous]
    [HttpPost]
    public async Task Registration(RegistrationModel model)
    {
        await service.RegistrationAsync(model);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<string> Login(LoginModel model)
    {
        return await service.LoginAsync(model);
    }

    [HttpGet]
    public async Task<string> UserInfo()
    {
        return await Task.Run(() => User.GetUserId());
    }
}