using H3WebAPI.Models;
using H3WebAPI.Models.Extensions;
using H3WebAPI.Models.RiotModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace H3WebAPI.Controllers.Internal
{
    public partial class TFTController : RiotAPIControllerExtension
    {
        /// <summary>
        /// Get Riot TFT League Summoner by Name
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
        [HttpGet("GetRiotTFTLeagueSummonerByName")]
        public async Task<IActionResult> GetRiotTFTLeagueSummonerByName(string userName, bool forceReload, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
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
            if (account != null && account.Id != null)
            {
                IActionResult leagueSummoner = await GetRiotTFTLeagueSummonerById(account.Id, forceReload);
                if (leagueSummoner != null && leagueSummoner is NotFoundObjectResult) // Not pretty, but functional
                {
                    return ReturnMessage(new RiotTFTLeagueSummonerAndSummoner
                    {
                        Summoner = account,
                        LeagueSummoner = null
                    });
                }
                else
                {
                    if (leagueSummoner != null)
                    {
                        return leagueSummoner;
                    }
                    else
                    {
                        return ReturnMessage(null);
                    }
                }
            }

            // Request Return
            return ReturnMessage(null);
        }
        
        /// <summary>
        /// Get Riot TFT League Summoner by Id
        /// </summary>
        /// <param name="summonerId"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
        [HttpGet("GetRiotTFTLeagueSummonerById")]
        public async Task<IActionResult> GetRiotTFTLeagueSummonerById(string summonerId, bool forceReload, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            if (string.IsNullOrWhiteSpace(summonerId))
            {
                return BadRequest("summonerId can not be empty or whitespace only");
            }

            RiotTFTLeagueSummonerAndSummoner? account = dataService.GetRiotTFTLeagueSummoner(summonerId);
            if (account == null || forceReload)
            {
                var tempAccount = await riotService.GetRiotTFTLeagueSummonerById(summonerId, platform);
                if (tempAccount != null)
                {
                    account = dataService.GetRiotTFTLeagueSummoner(summonerId);
                }
            }
            if (account == null || account != null && account.Summoner == null)
            {
                return NotFound("No league info was found with this summonerId");
            }

            // Request Return
            return ReturnMessage(account);
        }

        /// <summary>
        /// Get TFT League by type (RiotTFTLeagueType)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="forceReload"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
        [HttpGet("GetRiotTFTLeagueList")]
        public async Task<IActionResult> GetRiotTFTLeagueList(RiotTFTLeagueType type = RiotTFTLeagueType.challenger, bool forceReload = false, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            var leagueList = dataService.GetRiotTFTLeagueList(type, platform);

            if (leagueList == null || forceReload)
            {
                var temp = await riotService.GetRiotTFTLeagueByType(type, platform);

                if (temp != null)
                {
                    leagueList = dataService.GetRiotTFTLeagueList(type, platform);
                }
            }

            if (leagueList == null || leagueList != null && leagueList.Entries?.Count <= 0)
            {
                return NotFound("No league was Found");
            }

            // Visual Sorting
            var sortedList = leagueList?.Entries?.OrderByDescending(x => x.LeagueSummoner?.LeaguePoints)
                .ThenBy(x => x.LeagueSummoner?.Wins).ToList();

            if (sortedList != null && leagueList != null)
            {
                leagueList.Entries = sortedList;
            }

            // Request Return
            return ReturnMessage(leagueList);
        }


        /// <summary>
        /// Get TFT League by type (RiotTFTLeagueType)
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
        [HttpGet("GetRiotTFTLeagueTop3Mixed")]
        public async Task<IActionResult> GetRiotTFTLeagueTop3Mixed(RiotTFTAPIplatformEnum platformOverride = RiotTFTAPIplatformEnum.none)
        {
            var leagueList = dataService.GetRiotTFTLeagueTop3Mixed(platformOverride);

            if (leagueList == null || leagueList != null && leagueList?.Count < 3)
            {
                return NotFound("No top was Found");
            }

            if (leagueList != null)
            {
                foreach (var item in leagueList)
                {
                    if (item.LeagueSummoner != null && !string.IsNullOrEmpty(item.LeagueSummoner?.SummonerId) && item.Summoner?.AccountId == null && Enum.TryParse(item.LeagueSummoner.Region, out RiotTFTAPIplatformEnum localRegion))
                    {
                        item.Summoner = await riotService.GetRiotSummonerBySummonerId(item.LeagueSummoner.SummonerId, localRegion);
                    }   
                }
            }

            // Request Return
            return ReturnMessage(leagueList);
        }
        
        /// <summary>
        /// Get TFT League by type (RiotTFTLeagueType)
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        [Models.ApiVersion("1", "0", ApiVersionProductType.InternalAPI)]
        [HttpGet("GetRiotTFTLeagueTop3LP")]
        public async Task<IActionResult> GetRiotTFTLeagueTop3LP(RiotTFTAPIplatformEnum platformOverride = RiotTFTAPIplatformEnum.none)
        {
            var leagueList = dataService.GetRiotTFTLeagueTop3LP(platformOverride);

            if (leagueList == null || leagueList != null && leagueList?.Count < 3)
            {
                return NotFound("No top LP was Found");
            }

            if (leagueList != null)
            {
                foreach (var item in leagueList)
                {
                    if (item.LeagueSummoner != null && !string.IsNullOrEmpty(item.LeagueSummoner?.SummonerId) && item.Summoner?.AccountId == null && Enum.TryParse(item.LeagueSummoner?.Region, out RiotTFTAPIplatformEnum localRegion))
                    {
                        item.Summoner = await riotService.GetRiotSummonerBySummonerId(item.LeagueSummoner.SummonerId, localRegion);
                    }   
                }
            }

            // Request Return
            return ReturnMessage(leagueList);
        }
    }
}
