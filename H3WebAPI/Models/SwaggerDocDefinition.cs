namespace H3WebAPI.Models
{
    public class SwaggerDocDefinitionOptions
    {
        public const string SwaggerDocs = "SwaggerDocs";
        public List<SwaggerDefinitionData> Definitions { get; set; } = new();
    }

    public class SwaggerDefinitionData
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Version { get; set; }
        public string? Description { get; set; }
        public string? EndPoint { get; set; }
    }
}