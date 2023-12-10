using Newtonsoft.Json;

namespace H3WebAPI.Models.RiotModels
{
    public class RiotTFTStatusModel
    {
        [JsonPropertyAttribute("id")]
        public string? Id { get; set; }

        [JsonPropertyAttribute("name")]
        public string? Name { get; set; }

        [JsonPropertyAttribute("locales")]
        public List<string>? Locales { get; set; }

        //[JsonPropertyAttribute("maintenances")]
        //public List<string>? Maintenances { get; set; }

        //[JsonPropertyAttribute("incidents")]
        //public List<string>? Incidents { get; set; }
    }
}
