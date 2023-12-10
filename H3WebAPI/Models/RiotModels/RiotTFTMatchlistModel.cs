using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;

namespace H3WebAPI.Models.RiotModels
{
    public class RiotTFTMatchlistModel
    {
        [JsonPropertyAttribute("uniqueId")]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string? UniqueId { get; set; }

        [JsonPropertyAttribute("puuid")]
        public string? Puuid { get; set; }

        [JsonPropertyAttribute("matchlist")]
        public List<string>? Matchlist { get; set; }
    }
}
