using H3WebAPI.Models.Extensions;
using H3WebAPI.Models.RiotModels;
using H3WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace H3WebAPI.Tests
{
    [TestClass]
    public class UnitTest1
    {
        internal readonly RiotService? riotService;
        internal readonly DataService? dataService;
        internal readonly ILogger<RiotAPIControllerExtension>? logger;

        public UnitTest1()
        {
            var testConfiguration = new Dictionary<string, string>
            {
                {"XApiKey", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp"},
                {"RiotAPIKey", "RGAPI-ce411665-f765-491e-a8a5-90cc639f8f88"},
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(testConfiguration)
                .Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<DataService>();
            services.AddScoped<RiotService>();
            services.AddSingleton<DatabaseService>();
            services.AddSingleton<ConfigurationService>();
            services.AddLogging();

            var serviceProvider = services.BuildServiceProvider();

            if (serviceProvider != null)
            {
                riotService = serviceProvider.GetService<RiotService>();
                dataService = serviceProvider.GetService<DataService>();
                logger = serviceProvider.GetService<ILogger<RiotAPIControllerExtension>>();
            }
        }

        [TestMethod]
        public void SanityTest() // Sanity check to make sure test are actually being run
        {
            string? result = null;

            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task Test_RiotAccess()
        {
            if (riotService != null && dataService != null && logger != null) // Making sure our used services are available
            {
                var result = await riotService.TestRiotConnection();

                Assert.IsNotNull(result);

                Assert.IsTrue(result is RiotTFTStatusModel);
            }
        }

        private void CheckTFTSummoner(RiotTFTSummonerModel summoner)
        {
            Assert.IsNotNull(summoner);

            if (summoner != null)
            {
                Assert.IsNotNull(summoner.Name);
                StringAssert.Equals(string.IsNullOrEmpty(summoner.Name), string.IsNullOrEmpty(""));

                Assert.IsNotNull(summoner.Puuid);
                Assert.IsNotNull(summoner.Region);
                Assert.IsNotNull(summoner.ProfileIconId);
                Assert.IsNotNull(summoner.SummonerLevel);
            }
        }

        [TestMethod]
        public async Task Test_Bad_GetRiotSummonerByName()
        {
            if (riotService != null && dataService != null && logger != null) // Making sure our used services are available
            {
                var controller = new Controllers.Internal.TFTController(riotService, logger, dataService);

                // Known bad Summoner
                var result = await controller.GetRiotSummonerByName("Dextroid ioioioio");

                Assert.IsNotNull(result);
                Assert.IsTrue(result is NotFoundObjectResult);
            }
        }

        [TestMethod]
        public async Task Test_Good_GetRiotSummonerByName()
        {
            if (riotService != null && dataService != null && logger != null) // Making sure our used services are available
            {
                var controller = new Controllers.Internal.TFTController(riotService, logger, dataService);

                // Known good Summoner
                var result = await controller.GetRiotSummonerByName("Dextroid");

                Assert.IsNotNull(result);

                Assert.IsFalse(result is NotFoundObjectResult);
                Assert.IsTrue(result is OkObjectResult);

                if (result is OkObjectResult resultConverted)
                {
                    Assert.IsTrue(resultConverted.Value is RiotTFTSummonerModel);

                    if (resultConverted.Value != null && resultConverted.Value is RiotTFTSummonerModel summoner)
                    {
                        CheckTFTSummoner(summoner);
                    }
                }
            }
        }

        [TestMethod]
        public async Task Test_Bad_GetRiotSummonerListByName()
        {
            if (riotService != null && dataService != null && logger != null) // Making sure our used services are available
            {
                var controller = new Controllers.Internal.TFTController(riotService, logger, dataService);

                // Known bad Summoner
                var result = await controller.GetRiotSummonerListByName("Dextroid ioioioio");

                Assert.IsNotNull(result);

                Assert.IsTrue(result is NotFoundObjectResult);
            }
        }

        [TestMethod]
        public async Task Test_Good_GetRiotSummonerListByName()
        {
            if (riotService != null && dataService != null && logger != null) // Making sure our used services are available
            {
                var controller = new Controllers.Internal.TFTController(riotService, logger, dataService);

                // Known good Summoner
                var result = await controller.GetRiotSummonerListByName("Dextroid");

                Assert.IsNotNull(result);

                Assert.IsFalse(result is NotFoundObjectResult);
                Assert.IsTrue(result is OkObjectResult);

                if (result is OkObjectResult resultConverted && resultConverted.Value is List<RiotTFTSummonerModel> summoners)
                {
                    foreach (var summoner in summoners)
                    {
                        CheckTFTSummoner(summoner);
                    }
                }
            }
        }


        [TestMethod]
        public async Task Test_Good_GetRiotLeagueSummonerByName()
        {
            if (riotService != null && dataService != null && logger != null) // Making sure our used services are available
            {
                var controller = new Controllers.Internal.TFTController(riotService, logger, dataService);

                // Known good Summoner
                var result = await controller.GetRiotTFTLeagueSummonerByName("Dextroid", false);

                Assert.IsNotNull(result);

                Assert.IsFalse(result is NotFoundObjectResult);
                Assert.IsTrue(result is OkObjectResult);

                Console.WriteLine(result);

                if (result is OkObjectResult resultConverted)
                {
                    Assert.IsTrue(resultConverted.Value is RiotTFTLeagueSummonerAndSummoner);

                    if (resultConverted.Value != null && resultConverted.Value is RiotTFTLeagueSummonerAndSummoner summoner)
                    {
                        Assert.IsNotNull(summoner.Summoner);
                        if (summoner.Summoner != null)
                        {
                            CheckTFTSummoner(summoner.Summoner);
                        }
                    }
                }
            }
        }

    }
}