using H3WebAPI.Models;
using H3WebAPI.Models.RiotModels;
using Newtonsoft.Json;

namespace H3WebAPI.Services
{
    public class RiotService
    {
        private readonly DataService dataService;
        private readonly ConfigurationService configurationService;

        public RiotService(DataService dataService, ConfigurationService configurationService)
        {
            this.dataService = dataService;
            this.configurationService = configurationService;
        }

        public static string BaseUrl(RiotAPIPlatformsEnum platform) => !platform.Equals(RiotAPIPlatformsEnum.none) ? $"https://{platform}.api.riotgames.com" : $"https://api.riotgames.com";
        public static string BaseUrl(string platform) => !string.IsNullOrEmpty(platform) ? $"https://{platform}.api.riotgames.com" : $"https://api.riotgames.com";

        public async Task<HttpResponseMessage> GetFromUrl(string url)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "Gametrix-Game-Tracking-Project");

            var token = configurationService.GetRiotToken();
            client.DefaultRequestHeaders.Add("X-Riot-Token", token);

            return await client.GetAsync(url);
        }

        // HTTP Request Method for firing a call
        internal async Task<string> ExecuteHttpCall(StaticAPIDataItemModel callData)
        {
            string CallUrl = BaseUrl(callData.Platform.GetValueOrDefault()) + StaticAPIData.CreateUrlExtension(callData);

            if (!string.IsNullOrEmpty(callData.PlatformOverride))
            {
                CallUrl = BaseUrl(callData.PlatformOverride) + StaticAPIData.CreateUrlExtension(callData);
            }

            var responseMessage = await GetFromUrl(CallUrl);
            string jsonData = await responseMessage.Content.ReadAsStringAsync();

            return jsonData;
        }

        internal static T? ProcessResponseData<T>(string jsonData)
        {
            try
            {
                var resultData = JsonConvert.DeserializeObject<T>(jsonData);
                return resultData;
            }
            catch (Exception ex){
                Console.WriteLine(ex);
            }

            return default;
        }

        public async Task<RiotTFTStatusModel?> TestRiotConnection()
        {
            var callData = StaticAPIData.RiotTFTStatus;
            callData.PlatformOverride = RiotTFTAPIplatformEnum.euw1.ToString();

            // Fire the HTTP Request
            var result = await ExecuteHttpCall(callData);

            var convertedContent = ProcessResponseData<RiotTFTStatusModel>(result); // JSON to C#

            return convertedContent;
        }

        public async Task<RiotAccountModel?> GetRiotAccountByNameAndTag(string tagLine, string gameName)
        {
            if (!dataService.CanBeCalled())
            {
                return null;
            }

            var callData = StaticAPIData.RiotAccountByRiotId;
            callData.UrlParams = new string[] { gameName, tagLine };

            // Fire the HTTP Request
            var result = await ExecuteHttpCall(callData);

            if (!string.IsNullOrEmpty(result))
            {
                // Local Processing
                var convertedContent = ProcessResponseData<RiotAccountModel>(result); // JSON to C#

                if (convertedContent != null)
                {
                    dataService.UpdateRiotAccount(convertedContent);
                }

                return convertedContent;
            }

            return null;
        }
        
        public async Task<RiotTFTSummonerModel?> GetRiotSummonerByName(string userName, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            if (!dataService.CanBeCalled())
            {
                return null;
            }

            try
            {
                var callData = StaticAPIData.RiotSummonerByName;
                callData.PlatformOverride = platform.ToString();
                callData.UrlParams = new string[] { userName };

                // Fire the HTTP Request
                var result = await ExecuteHttpCall(callData);

                if (!string.IsNullOrEmpty(result))
                {
                    // Local Processing
                    var convertedContent = ProcessResponseData<RiotTFTSummonerModel>(result); // JSON to C#

                    if (convertedContent != null)
                    {
                        convertedContent.Region = platform.ToString();
                        dataService.UpdateRiotTFTSummonerAccount(convertedContent);
                    }

                    return convertedContent;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        public async Task<RiotTFTSummonerModel?> GetRiotSummonerByPuuid(string puuid, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            if (!dataService.CanBeCalled())
            {
                return null;
            }

            var callData = StaticAPIData.RiotSummonerByName;
            callData.PlatformOverride = platform.ToString();
            callData.UrlParams = new string[] { puuid };

            // Fire the HTTP Request
            var result = await ExecuteHttpCall(callData);

            if (!string.IsNullOrEmpty(result))
            {
                // Local Processing
                var convertedContent = ProcessResponseData<RiotTFTSummonerModel>(result); // JSON to C#

                if (convertedContent != null)
                {
                    convertedContent.Region = platform.ToString();
                    dataService.UpdateRiotTFTSummonerAccount(convertedContent);
                }

                return convertedContent;
            }

            return null;
        }
        public async Task<RiotTFTSummonerModel?> GetRiotSummonerBySummonerId(string summonerId, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            if (!dataService.CanBeCalled())
            {
                return null;
            }

            var callData = StaticAPIData.RiotSummonerBySummonerId;
            callData.PlatformOverride = platform.ToString();
            callData.UrlParams = new string[] { summonerId };

            // Fire the HTTP Request
            var result = await ExecuteHttpCall(callData);

            if (!string.IsNullOrEmpty(result))
            {
                // Local Processing
                var convertedContent = ProcessResponseData<RiotTFTSummonerModel>(result); // JSON to C#

                if (convertedContent != null)
                {
                    convertedContent.Region = platform.ToString();
                    dataService.UpdateRiotTFTSummonerAccount(convertedContent);
                }

                return convertedContent;
            }

            return null;
        }
        
        public async Task<List<string>?> GetRiotTFTMatchlistByPuuid(string puuid, int count = 20)
        {
            if (!dataService.CanBeCalled())
            {
                return null;
            }

            var callData = StaticAPIData.RiotTFTMatchlistByPuuid;
            callData.UrlParams = new string[] { $"{puuid}/ids", $"?count={count}" };

            // Fire the HTTP Request
            var result = await ExecuteHttpCall(callData);

            // Local Processing
            var convertedContent = ProcessResponseData<List<string>>(result); // JSON to C#

            if (convertedContent != null)
            {
                RiotTFTMatchlistModel userMatchlist = new RiotTFTMatchlistModel() { Puuid = puuid, Matchlist = convertedContent };

                dataService.UpdateRiotTFTSummonerMatchList(userMatchlist);
            }

            return convertedContent;
        }
        
        public async Task<RiotTFTMatchModel?> GetRiotTFTMatchByMatchId(string matchId, bool ignoreCall = false)
        {
            while (!dataService.CanBeCalled())
            {
            }

            var callData = StaticAPIData.RiotTFTMatchByMatchId;
            callData.UrlParams = new string[] { matchId };

            // Fire the HTTP Request
            var result = await ExecuteHttpCall(callData);

            // Local Processing
            var convertedContent = ProcessResponseData<RiotTFTMatchModel>(result); // JSON to C#

            if (convertedContent != null)
            {
                dataService.UpdateRiotTFTMatch(convertedContent);
            }

            return convertedContent;
        }
        
        public async Task<RiotTFTLeagueSummonerModel?> GetRiotTFTLeagueSummonerById(string summonerId, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            if (!dataService.CanBeCalled())
            {
                return null;
            }
            var callData = StaticAPIData.RiotTFTLeagueSummonerById;
            callData.PlatformOverride = platform.ToString();
            callData.UrlParams = new string[] { summonerId };

            // Fire the HTTP Request
            var result = await ExecuteHttpCall(callData);

            // Local Processing
            var convertedContent = ProcessResponseData<List<RiotTFTLeagueSummonerModel>>(result); // JSON to C#

            if (convertedContent != null)
            {
                foreach (var item in convertedContent)
                {
                    item.Region = platform.ToString();
                    dataService.UpdateRiotTFTLeagueSummoner(item, platform);
                }
            }

            return convertedContent?.FirstOrDefault();
        }
        
        public async Task<RiotTFTLeagueModel?> GetRiotTFTLeagueByType(RiotTFTLeagueType type = RiotTFTLeagueType.challenger, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            if (!dataService.CanBeCalled())
            {
                return null;
            }

            var callData = StaticAPIData.RiotTFTLeagueList;
            callData.PlatformOverride = platform.ToString();
            callData.UrlParams = new string[] { type.ToString() };

            // Fire the HTTP Request
            var result = await ExecuteHttpCall(callData);

            // Local Processing
            var convertedContent = ProcessResponseData<RiotTFTLeagueModel>(result); // JSON to C#

            try
            {
                if (convertedContent != null && !string.IsNullOrEmpty(convertedContent.LeagueId))
                {
                    convertedContent.Region = platform.ToString();
                    dataService.UpdateRiotTFTLeagueList(convertedContent, type, platform);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return convertedContent;
        }

    }
}
