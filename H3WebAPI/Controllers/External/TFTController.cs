using H3WebAPI.Models;
using H3WebAPI.Models.Extensions;
using H3WebAPI.Models.RiotModels;
using H3WebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;

namespace H3WebAPI.Controllers.External;

[Route("external")]
public class TFTController : RiotAPIControllerExtension
{
    public TFTController(RiotService riotService, ILogger<RiotAPIControllerExtension> logger, DataService dataService) : base(riotService, logger, dataService)
    {
    }

    /// <summary>
    /// Endpoint for getting a Riot TFT Summoner (Account) by name
    /// </summary>
    /// <param name="puuid"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.ExternalAPI)]
    [HttpGet("GetRiotSummonerByPuuid")]
    public async Task<IActionResult> GetRiotSummonerByPuuid(string puuid, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
    {
        if (string.IsNullOrWhiteSpace(puuid))
        {
            return BadRequest("puuid can not be empty or whitespace only");
        }

        var convertedContent = await riotService.GetRiotSummonerByPuuid(puuid, platform);
        if (convertedContent != null)
        {
            dataService.UpdateRiotTFTSummonerAccount(convertedContent);
        }

        // Request Return
        return ReturnMessage(convertedContent);
    }
    
    /// <summary>
    /// Endpoint for getting a Riot TFT Summoner (Account) by name
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.ExternalAPI)]
    [HttpGet("GetRiotSummonerByName")]
    public async Task<IActionResult> GetRiotSummonerByName(string userName, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return BadRequest("userName can not be empty or whitespace only");
        }

        var convertedContent = await riotService.GetRiotSummonerByName(userName, platform);
        if (convertedContent != null)
        {
            dataService.UpdateRiotTFTSummonerAccount(convertedContent);
        }

        // Request Return
        return ReturnMessage(convertedContent);
    }

    /// <summary>
    /// Get Riot TFT Matchlist by User Puuid (Count for how many to fetch)
    /// </summary>
    /// <param name="puuid"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.ExternalAPI)]
    [HttpGet("GetRiotTFTMatchlistByPuuid")]
    public async Task<IActionResult> GetRiotTFTMatchlistByPuuid(string puuid, int count = 20)
    {
        if (string.IsNullOrWhiteSpace(puuid))
        {
            return BadRequest("puuid can not be empty or whitespace only");
        }

        var convertedContent = await riotService.GetRiotTFTMatchlistByPuuid(puuid, count);
        if (convertedContent != null)
        {
            RiotTFTMatchlistModel userMatchlist = new RiotTFTMatchlistModel() { Puuid = puuid, Matchlist = convertedContent };

            dataService.UpdateRiotTFTSummonerMatchList(userMatchlist);
        }

        // Request Return
        return ReturnMessage(convertedContent);
    }

    /// <summary>
    /// Get TFT Match by MatchId
    /// </summary>
    /// <param name="matchId"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.ExternalAPI)]
    [HttpGet("GetRiotTFTMatchByMatchId")]
    public async Task<IActionResult> GetRiotTFTMatchByMatchId(string matchId)
    {
        var convertedContent = await riotService.GetRiotTFTMatchByMatchId(matchId);
        if (convertedContent != null)
        {
            dataService.UpdateRiotTFTMatch(convertedContent);
        }

        // Request Return
        return ReturnMessage(convertedContent);
    }

}

