using H3WebAPI.Models;
using H3WebAPI.Models.RiotModels;
using H3WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace H3WebAPI.Controllers;

[ApiController]
[Route("internal")]
[Produces("application/json")]
public class GameDataInternalController : ControllerBase
{
    private readonly DataService dataService;
    internal readonly RiotService riotService;
    private readonly ILogger<GameDataInternalController> logger;

    public GameDataInternalController(ILogger<GameDataInternalController> logger, DataService dataService, RiotService riotService)
    {
        this.logger = logger;
        this.dataService = dataService;
        this.riotService = riotService;
    }

    /// <summary>
    /// Get Riot Account by Tagline and GameName
    /// </summary>
    /// <param name="tagLine"></param>
    /// <param name="gameName"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
    [HttpGet("GetRiotAccountByTagLineAndGameName")]
    public async Task<IActionResult> GetRiotAccountByGameNameAndTagLine(string tagLine, string gameName, bool forceReload = false)
    {
        if (!StaticAPIExtensionMethods.StringIsMinimumLength(tagLine, 3))
        {
            return BadRequest("tagLine needs to be atleast 3 characters long");
        }
        if (string.IsNullOrWhiteSpace(gameName))
        {
            return BadRequest("gameName can not be empty or whitespace only");
        }

        RiotAccountModel? account = dataService.GetRiotAccountByNameAndTag(tagLine, gameName);
        if (account == null || forceReload)
        {
            account = await riotService.GetRiotAccountByNameAndTag(tagLine, gameName);
        }
        if (account == null || account != null && string.IsNullOrEmpty(account.PuuId))
        {
            return NotFound("No user was Found");
        }

        // Request Return
        return Ok(account);
    }
}
