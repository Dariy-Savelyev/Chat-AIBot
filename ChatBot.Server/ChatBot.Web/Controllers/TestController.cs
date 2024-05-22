using ChatBot.Application.Models;
using ChatBot.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController(ITestService service)
    : ControllerBase
{
    [HttpPost]
    public async Task<IEnumerable<TestModel>> Get(TestModel model)
    {
        return await service.GetAllAsync(model);
    }
}