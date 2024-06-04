using ChatBot.Application.Models.Tokens;
using ChatBot.Application.ServiceInterfaces;
using ChatBot.CrossCutting.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.Web.Controllers;

public class TokensController(ITokensService service) : BaseController
{
    [HttpPost]
    [Route("Refresh")]
    [AllowAnonymous]
    public async Task<ActionResult> Refresh(ValidateTokenModel request)
    {
        var user = await service.ValidateTokenAsync(request);

        if (user == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        var result = await service.RefreshTokenAsync(user);

        return new JsonResult(result);
    }

    [HttpPost]
    [Route("Revoke")]
    public async Task Revoke()
    {
        await service.RevokeTokenAsync(User.GetUserId());
    }
}