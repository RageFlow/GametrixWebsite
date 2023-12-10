namespace H3WebAPI.Services
{
    public class ConfigurationService
    {
        private readonly IConfiguration configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private string? RiotAPIToken { get; set; }
        private string RiotAPITokenPath = "RiotAPIKey";

        public string GetRiotToken()
        {
            if (string.IsNullOrEmpty(RiotAPIToken))
            {
                RiotAPIToken = configuration.GetValue<string>(RiotAPITokenPath);
            }

            return RiotAPIToken;
        }

    }
}
