using H3WebAPI.Models.RiotModels;
using MongoDB.Driver;

namespace H3WebAPI.Models
{
    public static class StaticAPIData
    {
        public static StaticAPIDataItemModel RiotTFTStatus = new StaticAPIDataItemModel("/tft/status/v1/platform-data")
        {
            Platform = RiotAPIPlatformsEnum.none,
        };
        
        public static StaticAPIDataItemModel RiotAccountByRiotId = new StaticAPIDataItemModel("/riot/account/v1/accounts/by-riot-id")
        {
            Platform = RiotAPIPlatformsEnum.europe,
        };

        public static StaticAPIDataItemModel RiotSummonerByName = new StaticAPIDataItemModel("/tft/summoner/v1/summoners/by-name")
        {
            Platform = RiotAPIPlatformsEnum.none,
        };
        public static StaticAPIDataItemModel RiotSummonerByPuuid = new StaticAPIDataItemModel("/tft/summoner/v1/summoners/by-puuid")
        {
            Platform = RiotAPIPlatformsEnum.none,
        };
        public static StaticAPIDataItemModel RiotSummonerBySummonerId = new StaticAPIDataItemModel("/tft/summoner/v1/summoners")
        {
            Platform = RiotAPIPlatformsEnum.none
        };

        public static StaticAPIDataItemModel RiotTFTMatchlistByPuuid = new StaticAPIDataItemModel("/tft/match/v1/matches/by-puuid")
        {
            Platform = RiotAPIPlatformsEnum.europe,
        };
        
        public static StaticAPIDataItemModel RiotTFTMatchByMatchId = new StaticAPIDataItemModel("/tft/match/v1/matches")
        {
            Platform = RiotAPIPlatformsEnum.europe,
        };
        
        public static StaticAPIDataItemModel RiotTFTLeagueList = new StaticAPIDataItemModel("/tft/league/v1/") // Needs "challenger"/"grandmaster" etc behind url
        {
            Platform = RiotAPIPlatformsEnum.none,
        };
        
        public static StaticAPIDataItemModel RiotTFTLeagueSummonerById = new StaticAPIDataItemModel("/tft/league/v1/entries/by-summoner")
        {
            Platform = RiotAPIPlatformsEnum.none,
        };

        public static string CreateUrlExtension(StaticAPIDataItemModel data)
        {
            return (data.Url + data.GetParams()).Replace(" ", "").Replace("//", "/");
        }

        public static ProjectionDefinition<RiotTFTLeagueSummonerModelWithExtra, RiotTFTLeagueSummonerAndSummoner> CustomLeagueSummonerProjection = Builders<RiotTFTLeagueSummonerModelWithExtra>.Projection.Expression(u =>
            new RiotTFTLeagueSummonerAndSummoner
            {
                Summoner = u.Summoners != null ? u.Summoners.ElementAt(0) : null,
                LeagueSummoner = new RiotTFTLeagueSummonerModel
                {
                    Region = u.Region,
                    LeagueId = u.LeagueId,
                    SummonerId = u.SummonerId,
                    SummonerName = u.SummonerName,
                    QueueType = u.QueueType,
                    RatedTier = u.RatedTier,
                    RatedRating = u.RatedRating,
                    Tier = u.Tier,
                    Rank = u.Rank,
                    LeaguePoints = u.LeaguePoints,
                    Wins = u.Wins,
                    Losses = u.Losses,
                    HotStreak = u.HotStreak,
                    Veteran = u.Veteran,
                    FreshBlood = u.FreshBlood,
                    Inactive = u.Inactive,
                    MiniSeries = u.MiniSeries
                }
            });
        
        //public static ProjectionDefinition<RiotTFTLeagueSummonerModelWithExtra, RiotTFTLeagueSummonerModel> CustomLeagueSummonerProjection = Builders<RiotTFTLeagueSummonerModelWithExtra>.Projection.Expression(u =>
        //    new RiotTFTLeagueSummonerModel
        //    {
        //        Region = u.Region,
        //        LeagueId = u.LeagueId,
        //        SummonerId = u.SummonerId,
        //        SummonerName = u.SummonerName,
        //        QueueType = u.QueueType,
        //        RatedTier = u.RatedTier,
        //        RatedRating = u.RatedRating,
        //        Tier = u.Tier,
        //        Rank = u.Rank,
        //        LeaguePoints = u.LeaguePoints,
        //        Wins = u.Wins,
        //        Losses = u.Losses,
        //        HotStreak = u.HotStreak,
        //        Veteran = u.Veteran,
        //        FreshBlood = u.FreshBlood,
        //        Inactive = u.Inactive,
        //        MiniSeries = u.MiniSeries,
        //        Summoner = u.Summoners != null ? u.Summoners.ElementAt(0) : null
        //    });
    }

    public class StaticAPIDataItemModel
    {
        public StaticAPIDataItemModel(string Url)
        {
            this.Url = Url;
        }

        public RiotAPIPlatformsEnum? Platform { get; set; }
        public string? PlatformOverride { get; set; }
        public string Url { get; private set; } = string.Empty;
        public string[]? UrlParams { get; set; }

        public string GetParams()
        {
            if (UrlParams != null)
            {
                return String.Join(string.Empty, UrlParams.Select(x => $"/{x}"));
            }
            else
            {
                return String.Empty;
            }
        }
    }

    public static class StaticAPIExtensionMethods
    {
        public static bool StringIsMinimumLength(string value, int length = 0)
        {
            if (value.Length >= length)
            {
                return true;
            }
            return false;
        }
    }
}
