using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace H3WebAPI.Models.RiotModels
{
    public class RiotTFTSummonerModel
    {
        [JsonPropertyAttribute("uniqueId")]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string? UniqueId { get; set; }

        [JsonPropertyAttribute("accountId")]
        public string? AccountId { get; set; }
        
        [JsonIgnore]
        public string? Region { get; set; }

        [JsonPropertyAttribute("profileIconId")]
        public int ProfileIconId { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        [JsonIgnore]
        public string ProfileIconUrl
        {
            get { return $"https://website.dk/gametrix/profileicon/{ProfileIconId}.png"; }
        }

        [JsonPropertyAttribute("revisionDate")]
        public long RevisionDate { get; set; }

        [JsonPropertyAttribute("name")]
        public string? Name { get; set; }

        [JsonPropertyAttribute("id")]
        public string? Id { get; set; }

        [JsonPropertyAttribute("puuid")]        
        public string? Puuid { get; set; }

        [JsonPropertyAttribute("summonerLevel")]
        public long SummonerLevel { get; set; }
    }
}
