using ChatBot.CrossCutting.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

public class HomeController(ILogger<HomeController> logger) : BaseController
{
    [AllowAnonymous]
    [HttpGet]
    public string? AppVersion()
    {
        return Environment.GetEnvironmentVariable(ConfigurationConstants.AppVersion);
    }
}