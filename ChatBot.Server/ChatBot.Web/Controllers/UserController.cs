using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

[ApiController]
[Route("api/account/registration")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpPost]
    public async Task<bool> Registration(RegistrationModel model)
    {
       return await service.RegistrationAsync(model);
    }
}