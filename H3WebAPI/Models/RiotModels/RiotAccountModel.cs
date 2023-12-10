using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;

namespace H3WebAPI.Models.RiotModels
{
    public class RiotAccountModel
    {
        [JsonPropertyAttribute("uniqueId")]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonIgnoreIfDefault]
        public string? UniqueId { get; }

        [JsonPropertyAttribute("puuId")]
        public string? PuuId { get; set; }

        [JsonPropertyAttribute("gameName")]
        public string? GameName { get; set; }

        [JsonPropertyAttribute("tagLine")]
        public string? TagLine { get; set; }
    }
}
