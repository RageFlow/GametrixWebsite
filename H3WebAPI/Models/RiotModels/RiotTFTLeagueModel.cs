using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;

namespace H3WebAPI.Models.RiotModels
{
    public enum RiotTFTLeagueType
    {
        challenger,
        grandmaster,
        master
    }

    public class RiotTFTLeagueModel
    {
        [JsonPropertyAttribute("uniqueId")]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string? UniqueId { get; set; }

        [JsonIgnore]
        public string? Region { get; set; }

        [JsonPropertyAttribute("leagueId")]
        public string? LeagueId { get; set; }

        [JsonPropertyAttribute("entries")]
        [BsonIgnore]
        public List<RiotTFTLeagueSummonerAndSummoner>? Entries { get; set; }

        [JsonPropertyAttribute("tier")]
        public string? Tier { get; set; }

        [JsonPropertyAttribute("name")]
        public string? Name { get; set; }

        [JsonPropertyAttribute("queue")]
        public string? Queue { get; set; }
    }

    public class RiotTFTLeagueModelMiniseries
    {
        [JsonPropertyAttribute("losses")]
        public int? Losses { get; set; }

        [JsonPropertyAttribute("progress")]
        public string? Progress { get; set; }

        [JsonPropertyAttribute("target")]
        public int? Target { get; set; }

        [JsonPropertyAttribute("wins")]
        public int? Wins { get; set; }
    }
}
