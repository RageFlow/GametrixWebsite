using H3WebAPI.Models;
using H3WebAPI.Models.Extensions;
using H3WebAPI.Models.RiotModels;
using H3WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace H3WebAPI.Controllers.Internal;

[Route("internal")]
public partial class TFTController : RiotAPIControllerExtension
{
    public TFTController(RiotService riotService, ILogger<RiotAPIControllerExtension> logger, DataService dataService) : base(riotService, logger, dataService)
    {
    }

    /// <summary>
    /// Endpoint for getting a Riot TFT Summoner (Account) by name
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
    [HttpGet("GetRiotSummonerByName")]
    public async Task<IActionResult> GetRiotSummonerByName(string userName, bool forceReload = false, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return BadRequest("userName can not be empty or whitespace only");
        }

        RiotTFTSummonerModel? account = dataService.GetRiotTFTSummonerAccountByName(userName, platform);
        if (account == null || forceReload)
        {
            account = await riotService.GetRiotSummonerByName(userName, platform);
        }
        if (account == null || account != null && string.IsNullOrEmpty(account.Puuid))
        {
            return NotFound("No user was Found");
        }

        // Request Return
        return ReturnMessage(account);
    }
    
    /// <summary>
    /// Endpoint for getting a Riot TFT Summoner (Account) by name
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
    [HttpGet("GetRiotSummonerListByName")]
    public async Task<IActionResult> GetRiotSummonerListByName(string userName, bool forceReload = false, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            return BadRequest("userName can not be empty or whitespace only");
        }

        List<RiotTFTSummonerModel>? accounts = dataService.GetRiotTFTSummonerAccountListByName(userName, platform);

        if (accounts != null && !accounts.Any(x => x.Name != null && x.Name.ToLower().Equals(userName.ToLower()) && x.Region == platform.ToString()) || forceReload)
        {
            var newAccount = await riotService.GetRiotSummonerByName(userName, platform);

            if (newAccount != null && !string.IsNullOrEmpty(newAccount.AccountId) && accounts != null)
            {
                var oldAccount = accounts.Where(x => x.Name != null && x.Name.ToLower().Equals(userName.ToLower()) && x.Region == platform.ToString()).FirstOrDefault();
                if (oldAccount != null)
                {
                    accounts.Remove(oldAccount);
                }
                accounts.Add(newAccount);
            }
        }
        if (accounts == null || accounts != null && accounts.Count() == 0)
        {
            return NotFound("No users was Found");
        }

        // Visual Sorting
        var sortedList = accounts?.OrderBy(x => x.Name?.Length)
            .ThenBy(x => x.Name).ToList();

        // Request Return
        return ReturnMessage(sortedList);
    }

    /// <summary>
    /// Endpoint for getting a Riot TFT Summoner (Account) by puuid
    /// </summary>
    /// <param name="puuid"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
    [HttpGet("GetRiotSummonerByPuuid")]
    public async Task<IActionResult> GetRiotSummonerByPuuid(string puuid, bool forceReload = false, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
    {
        if (string.IsNullOrWhiteSpace(puuid))
        {
            return BadRequest("puuid can not be empty or whitespace only");
        }

        var account = dataService.GetRiotTFTSummonerAccountByPuuid(puuid);
        if (account == null || forceReload)
        {
            account = await riotService.GetRiotSummonerByPuuid(puuid, platform);
        }
        if (account == null || account != null && string.IsNullOrEmpty(account.Puuid))
        {
            return NotFound("No summoner was Found");
        }

        // Request Return
        return ReturnMessage(account);
    }
    
    /// <summary>
    /// Endpoint for getting a Riot TFT Summoner (Account) by puuid
    /// </summary>
    /// <param name="summonerId"></param>
    /// <param name="platform"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
    [HttpGet("GetRiotSummonerBySummonerId")]
    public async Task<IActionResult> GetRiotSummonerBySummonerId(string summonerId, bool forceReload = false, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
    {
        if (string.IsNullOrWhiteSpace(summonerId))
        {
            return BadRequest("puuid can not be empty or whitespace only");
        }

        var account = dataService.GetRiotTFTSummonerAccountBySummonerId(summonerId);
        if (account == null || forceReload)
        {
            account = await riotService.GetRiotSummonerBySummonerId(summonerId, platform);
        }
        if (account == null || account != null && string.IsNullOrEmpty(account.Puuid))
        {
            return NotFound("No summoner was Found");
        }

        // Request Return
        return ReturnMessage(account);
    }

    

    /// <summary>
    /// Get Riot TFT Matchlist by User Puuid (Count for how many to fetch)
    /// </summary>
    /// <param name="puuid"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
    [HttpGet("GetRiotTFTMatchlistByPuuid")]
    public async Task<IActionResult> GetRiotTFTMatchlistByPuuid(string puuid, int count = 50, bool forceReload = false)
    {
        if (string.IsNullOrWhiteSpace(puuid))
        {
            return BadRequest("puuid can not be empty or whitespace only");
        }

        var matchList = dataService.GetRiotTFTSummonerMatchList(puuid);

        if (matchList == null || forceReload)
        {
            var listAccount = await riotService.GetRiotTFTMatchlistByPuuid(puuid, count);
            matchList = new RiotTFTMatchlistModel() { Puuid = puuid, Matchlist = listAccount ?? new List<string>() };
        }

        if (matchList == null || matchList != null && matchList.Matchlist?.Count() == 0 )
        {
            return NotFound("No match list on this Puuid was Found");
        }

        // Request Return
        return ReturnMessage(matchList?.Matchlist);
    }
    [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
    [HttpGet("GetRiotTFTMatchesByPuuid")]
    public async Task<IActionResult> GetRiotTFTMatchesByPuuid(string puuid, int count = 10, int start = 0, bool forceReload = false)
    {
        if (string.IsNullOrWhiteSpace(puuid))
        {
            return BadRequest("puuid can not be empty or whitespace only");
        }

        if (count > 10) // Max limit of count
        {
            count = 10;
        }
        if (start > 50) // Max start to fit count
        {
            start = 50;
        }

        var matchList = dataService.GetRiotTFTSummonerMatchList(puuid);

        if (matchList == null || forceReload)
        {
            var listAccount = await riotService.GetRiotTFTMatchlistByPuuid(puuid, 50);
            matchList = new RiotTFTMatchlistModel() { Puuid = puuid, Matchlist = listAccount ?? new List<string>() };
        }

        if (matchList == null || matchList != null && matchList.Matchlist?.Count() == 0 )
        {
            return NotFound("No match list on this Puuid was Found");
        }

        Range range = new Range(start, start + count);
        var TempMatchlist = matchList?.Matchlist?.Take(range).ToList();

        List<RiotTFTMatchModel> allMatches = new();

        if (TempMatchlist != null)
        {
            ConcurrentBag<Task<RiotTFTMatchModel?>> ToDo = new();
            foreach (var match in TempMatchlist)
            {
                ToDo.Add(GetRiotTFTMatch(match));
            }
            await Task.WhenAll(ToDo);

            foreach (var item in ToDo)
            {
                var test = await item;
                if (test != null)
                {
                    allMatches.Add(test);
                }
            }
        }

        RiotTFTMatchesModel export = new RiotTFTMatchesModel()
        {
            Count = count,
            Start = start,
            Puuid = puuid,
            Matches = allMatches.OrderByDescending(x => x.Info.Game_datetime).ToList()
        };

        // Request Return
        return ReturnMessage(export);
    }


    /// <summary>
    /// Get TFT Match by MatchId
    /// </summary>
    /// <param name="matchId"></param>
    /// <returns></returns>
    [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
    [HttpGet("GetRiotTFTMatchByMatchId")]
    public async Task<IActionResult> GetRiotTFTMatchByMatchId(string matchId, bool forceReload = false)
    {
        if (string.IsNullOrWhiteSpace(matchId))
        {
            return BadRequest("matchId can not be empty or whitespace only");
        }

        var match = await GetRiotTFTMatch(matchId, forceReload);

        if (match == null)
        {
            return NotFound("No match on this Id was Found");
        }

        // Request Return
        return ReturnMessage(match);
    }
    
    private async Task<RiotTFTMatchModel?> GetRiotTFTMatch(string matchId, bool forceReload = false)
    {
        var match = dataService.GetRiotTFTMatch(matchId);
        if (match == null || forceReload)
        {
            match = await riotService.GetRiotTFTMatchByMatchId(matchId, true);
        }
        return match;
    }

}

