using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace H3WebAPI.Models.RiotModels
{
    public class RiotTFTLeagueSummonerAndSummoner
    {
        public RiotTFTSummonerModel? Summoner { get; set; }

        public RiotTFTLeagueSummonerModel? LeagueSummoner { get; set; }
    }

    public class RiotTFTLeagueSummonerModelWithExtra : RiotTFTLeagueSummonerModel
    {
        [JsonPropertyAttribute("summoners")]
        [JsonIgnore]
        public IEnumerable<RiotTFTSummonerModel>? Summoners { get; set; }
    }

    public class RiotTFTLeagueSummonerModel
    {
        [JsonPropertyAttribute("uniqueId")]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string? UniqueId { get; set; }

        [JsonIgnore]
        public string? Region { get; set; }
        
        [JsonPropertyAttribute("leagueId")]
        public string? LeagueId { get; set; }

        [JsonPropertyAttribute("summonerId")]
        public string? SummonerId { get; set; }

        [JsonPropertyAttribute("summonerName")]
        public string? SummonerName { get; set; }

        [JsonPropertyAttribute("queueType")]
        public string? QueueType { get; set; }

        [JsonPropertyAttribute("ratedTier")]
        public string? RatedTier { get; set; }

        [JsonPropertyAttribute("ratedRating")]
        public int RatedRating { get; set; }

        [JsonPropertyAttribute("tier")]
        public string? Tier { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        [JsonIgnore]
        public string TierIconUrl
        {
            get { return !string.IsNullOrEmpty(Tier) ? $"https://website.dk/gametrix/tft_rank/TFT_Regalia_{Tier}.png" : "https://website.dk/gametrix/tft_rank/TFT_Regalia_Provisional.png"; }
        }

        [JsonPropertyAttribute("rank")]
        public string? Rank { get; set; }

        [JsonPropertyAttribute("leaguePoints")]
        public int LeaguePoints { get; set; }

        [JsonPropertyAttribute("wins")]
        public int Wins { get; set; }

        [JsonPropertyAttribute("winrate")]
        private double _winrate;
        public double WinRate {
            get { return (double)Wins / (double)(Wins + Losses) * 100d; }
            set { _winrate = (double)Wins / (double)(Wins + Losses) * 100d; }
        }

        [JsonPropertyAttribute("losses")]
        public int Losses { get; set; }

        [JsonPropertyAttribute("hotStreak")]
        public bool HotStreak { get; set; }

        [JsonPropertyAttribute("veteran")]
        public bool Veteran { get; set; }

        [JsonPropertyAttribute("freshBlood")]
        public bool FreshBlood { get; set; }

        [JsonPropertyAttribute("inactive")]
        public bool Inactive { get; set; }

        [JsonPropertyAttribute("miniSeries")]
        public RiotTFTLeagueModelMiniseries? MiniSeries { get; set; }

        [JsonPropertyAttribute("summoner")]
        [JsonIgnore]
        public RiotTFTSummonerModel? Summoner { get; set; }
    }
}
