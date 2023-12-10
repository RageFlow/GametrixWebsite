using H3WebAPI.Models;
using H3WebAPI.Models.Extensions;
using H3WebAPI.Models.RiotModels;
using H3WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace H3WebAPI.Controllers;

[Route("external")]
public class GameDataExternalController : RiotAPIControllerExtension
{
    public GameDataExternalController(RiotService riotService, ILogger<RiotAPIControllerExtension> logger, DataService dataService) : base(riotService, logger, dataService)
    {
    }

    /// <summary>
    /// Get Riot Account by Tagline and GameName
    /// </summary>
    /// <param name="tagLine"></param>
    /// <param name="gameName"></param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Models.ApiVersion("1", "0", ApiVersionProductType.ExternalAPI)]
    [HttpGet("GetRiotAccountByTagLineAndGameName")]
    public async Task<IActionResult> GetRiotAccountByGameNameAndTagLine(string tagLine, string gameName)
    {
        if (!StaticAPIExtensionMethods.StringIsMinimumLength(tagLine, 3))
        {
            return BadRequest("tagLine needs to be atleast 3 characters long");
        }
        if (string.IsNullOrWhiteSpace(gameName))
        {
            return BadRequest("gameName can not be empty or whitespace only");
        }

        var callData = StaticAPIData.RiotAccountByRiotId;
        callData.UrlParams = new string[] { gameName, tagLine };

        // Fire the HTTP Request
        var result = await riotService.ExecuteHttpCall(callData);

        // Local Processing
        var convertedContent = RiotService.ProcessResponseData<RiotAccountModel>(result); // JSON to C#

        if (convertedContent != null)
        {
            return ReturnMessage( convertedContent);
        }

        // Request Return
        return ReturnMessage(result);
    }
}   
