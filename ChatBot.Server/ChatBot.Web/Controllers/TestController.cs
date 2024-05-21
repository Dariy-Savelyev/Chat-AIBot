using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController(ITestService service)
    : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<TestModel>> Get()
    {
        return await service.GetAllAsync();
    }
}