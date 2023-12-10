using Microsoft.AspNetCore.Components.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System.Reflection;

namespace H3WebAPI.Models.RiotModels
{
    public class RiotTFTMatchModel
    {
        public RiotTFTMatchModel(RiotTFTMatchMetaModel metadata, RiotTFTMatchInfoModel info)
        {
            Metadata = metadata;
            Info = info;
        }

        [JsonPropertyAttribute("uniqueId")]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string? UniqueId { get; set; }

        [JsonPropertyAttribute("metadata")]
        public RiotTFTMatchMetaModel Metadata { get; set; }

        [JsonPropertyAttribute("info")]
        public RiotTFTMatchInfoModel Info { get; set; }
    }

    public class RiotTFTMatchMetaModel
    {
        [JsonPropertyAttribute("data_version")]
        public string? Data_version { get; set; }

        [JsonPropertyAttribute("match_id")]
        public string? Match_id { get; set; }

        [JsonPropertyAttribute("participants")]
        public List<string>? Participants { get; set; }
    }

    public class RiotTFTMatchInfoModel
    {
        [JsonPropertyAttribute("game_datetime")]
        public long Game_datetime { get; set; }

        [JsonPropertyAttribute("game_length")]
        public float Game_length { get; set; }

        [JsonPropertyAttribute("game_variation")]
        public string? Game_variation { get; set; }

        [JsonPropertyAttribute("game_version")]
        public string? Game_version { get; set; }

        [JsonPropertyAttribute("participants")]
        public List<RiotTFTMatchParticipantsModel>? Participants { get; set; }

        [JsonPropertyAttribute("queue_id")]
        public int? Queue_id { get; set; }

        [JsonPropertyAttribute("tft_set_number")]
        public int? Tft_set_number { get; set; }
    }

    public class RiotTFTMatchParticipantsModel
    {
        [JsonPropertyAttribute("augments")]
        public List<string>? Augments { get; set; }

        [JsonPropertyAttribute("companion")]
        public RiotTFTCompanionModel? Companion { get; set; }

        [JsonPropertyAttribute("gold_left")]
        public int Gold_left { get; set; }

        [JsonPropertyAttribute("last_round")]
        public int Last_round { get; set; }

        [JsonPropertyAttribute("level")]
        public int Level { get; set; }

        [JsonPropertyAttribute("placement")]
        public int Placement { get; set; }

        [JsonPropertyAttribute("players_eliminated")]
        public int Players_eliminated { get; set; }

        [JsonPropertyAttribute("puuid")]
        public string? Puuid { get; set; }

        [JsonPropertyAttribute("time_eliminated")]
        public float Time_eliminated { get; set; }

        [JsonPropertyAttribute("total_damage_to_players")]
        public int Total_damage_to_players { get; set; }

        [JsonPropertyAttribute("traits")]
        public List<RiotTFTTraitModel>? Traits { get; set; }

        [JsonPropertyAttribute("units")]
        public List<RiotTFTUnitModel>? Units { get; set; }
    }

    public class RiotTFTCompanionModel
    {
        [JsonPropertyAttribute("content_ID")]
        public string? Content_ID { get; set; }

        [JsonPropertyAttribute("skin_ID")]
        public int Skin_ID { get; set; }

        [JsonPropertyAttribute("species")]
        public string? Species { get; set; }
    }

    public class RiotTFTTraitModel
    {
        [JsonPropertyAttribute("name")]
        public string? Name { get; set; }

        [JsonPropertyAttribute("num_units")]
        public int Num_units { get; set; }

        [JsonPropertyAttribute("style")]
        public int Style { get; set; }

        [JsonPropertyAttribute("tier_current")]
        public int Tier_current { get; set; }

        [JsonPropertyAttribute("tier_total")]
        public int Tier_total { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        [JsonIgnore]
        public string Trait_name
        {
            get { return $"{Name?.Split('_')?[1]}"; }
        }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        [JsonIgnore]
        public string Trait_url
        {
            get { return $"https://website.dk/gametrix/tft_trait/Trait_Icon_9_{Name?.Split('_')?[1]}.png"; }
        }
    }
    
    public class RiotTFTUnitModel
    {
        [JsonPropertyAttribute("items")]
        public List<int>? Items { get; set; }

        [JsonPropertyAttribute("character_id")]
        public string? Character_id { get; set; }

        [JsonPropertyAttribute("chosen")]
        public string? Chosen { get; set; }

        [JsonPropertyAttribute("name")]
        public string? Name { get; set; }

        [JsonPropertyAttribute("rarity")]
        public int Rarity { get; set; }

        [JsonPropertyAttribute("tier")]
        public int Tier { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        [JsonIgnore]
        public string Unit_name
        {
            get { return $"{Character_id?.Split('_')?[1]}"; }
        }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        [JsonIgnore]
        public string Unit_url
        {
            get { return $"https://website.dk/gametrix/champion/{Character_id?.Split('_')?[1]}.png"; }
        }
    }
}
