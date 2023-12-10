using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;

namespace H3WebAPI.Models.RiotModels
{
    public class RiotTFTMatchesModel
    {
        [JsonPropertyAttribute("uniqueId")]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string? UniqueId { get; set; }

        [JsonPropertyAttribute("puuid")]
        public string? Puuid { get; set; }
        
        [JsonPropertyAttribute("count")]
        public int Count { get; set; }
        
        [JsonPropertyAttribute("start")]
        public int Start { get; set; }

        [JsonPropertyAttribute("matchlist")]
        public List<RiotTFTMatchModel>? Matches { get; set; }
    }
}
