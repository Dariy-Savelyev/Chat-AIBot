using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class BaseController : ControllerBase
{
}