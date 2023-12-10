using H3WebAPI.Models;
using H3WebAPI.Models.Extensions;
using H3WebAPI.Models.RiotModels;
using MongoDB.Driver;

namespace H3WebAPI.Services
{
    public class DataService
    {
        private readonly DatabaseService databaseService;
        internal readonly ILogger<DatabaseService> logger;

        public DataService(DatabaseService databaseService, ILogger<DatabaseService> logger)
        {
            this.databaseService = databaseService;
            this.logger = logger;
        }

        public bool CanBeCalled()
        {
            return databaseService.CanBeCalled();
        }

        public MongoCollectionBase<T> GetMongoCollection<T>(string collectionName = "tft_base", string databaseName = "tft_table")
        {
            MongoClient dbClient = databaseService.InitDatabaseCon();

            var database = dbClient.GetDatabase(databaseName);

            return (MongoCollectionBase<T>)database.GetCollection<T>(collectionName);
        }

        private void UpdateItem<T>(T item, MongoCollectionBase<T> collection, FilterDefinition<T> filter, UpdateDefinition<T>? update = null)
        {
            if (item != null)
            {
                if (update != null)
                {
                    UpdateOptions updateOptions = new UpdateOptions() { IsUpsert = true };
                    collection.UpdateOne(filter, update, updateOptions);
                }
                else
                {
                    ReplaceOptions replaceOptions = new ReplaceOptions() { IsUpsert = true };
                    collection.ReplaceOne(filter, item, replaceOptions);
                }
            }
        }

        public RiotAccountModel? GetRiotAccountByNameAndTag(string tag, string name)
        {
            var collection = GetMongoCollection<RiotAccountModel>("riot_account", "riot_table");

            var filter = Builders<RiotAccountModel>.Filter
                .Where(account => account.GameName == name && account.TagLine == tag);

            var existing = collection.Find(filter).FirstOrDefault();

            return existing;
        }
        public void UpdateRiotAccount(RiotAccountModel user)
        {
            if (!string.IsNullOrEmpty(user.PuuId))
            {
                var collection = GetMongoCollection<RiotAccountModel>("riot_account", "riot_table");
                var filter = Builders<RiotAccountModel>.Filter
                    .Where(account => account.GameName == user.GameName && account.TagLine == user.TagLine);

                UpdateItem(user, collection, filter);
            }
        }

        public RiotTFTSummonerModel? GetRiotTFTSummonerAccountByName(string userName, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            var collection = GetMongoCollection<RiotTFTSummonerModel>("tft_summoner");

            var filter = Builders<RiotTFTSummonerModel>.Filter
                .Where(summoner => summoner.Name != null &&
                    summoner.Name.ToLower().Equals(userName.ToLower()) &&
                    summoner.Region == platform.ToString()
                );

            var existing = collection.Find(filter).FirstOrDefault();

            return existing;
        }
        public List<RiotTFTSummonerModel>? GetRiotTFTSummonerAccountListByName(string userName, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            var collection = GetMongoCollection<RiotTFTSummonerModel>("tft_summoner");

            var filter = Builders<RiotTFTSummonerModel>.Filter
                .Where(summoner => summoner.Name != null &&
                    summoner.Name.ToLower().Contains(userName.ToLower()) &&
                    summoner.Region == platform.ToString()
                );

            var existing = collection.Find(filter).ToList();

            return existing;
        }
        public RiotTFTSummonerModel GetRiotTFTSummonerAccountByPuuid(string puuid)
        {
            var collection = GetMongoCollection<RiotTFTSummonerModel>("tft_summoner");

            var filter = Builders<RiotTFTSummonerModel>.Filter
                .Eq(summoner => summoner.Puuid, puuid);

            var existing = collection.Find(filter).FirstOrDefault();

            return existing;
        }
        public RiotTFTSummonerModel GetRiotTFTSummonerAccountBySummonerId(string summonerId)
        {
            var collection = GetMongoCollection<RiotTFTSummonerModel>("tft_summoner");

            var filter = Builders<RiotTFTSummonerModel>.Filter
                .Eq(summoner => summoner.Id, summonerId);

            var existing = collection.Find(filter).FirstOrDefault();

            return existing;
        }
        public void UpdateRiotTFTSummonerAccount(RiotTFTSummonerModel user)
        {
            if (!string.IsNullOrEmpty(user.Id))
            {
                var collection = GetMongoCollection<RiotTFTSummonerModel>("tft_summoner");

                var filter = Builders<RiotTFTSummonerModel>.Filter
                    .Eq(summoner => summoner.Puuid, user.Puuid);

                UpdateItem(user, collection, filter);
            }
        }

        public RiotTFTMatchlistModel GetRiotTFTSummonerMatchList(string puuid)
        {
            var collection = GetMongoCollection<RiotTFTMatchlistModel>("tft_summoner_matchlist");

            var filter = Builders<RiotTFTMatchlistModel>.Filter
                .Eq(summoner => summoner.Puuid, puuid);

            var existing = collection.Find(filter).FirstOrDefault();

            return existing;
        }
        public void UpdateRiotTFTSummonerMatchList(RiotTFTMatchlistModel userMatchlist)
        {
            var collection = GetMongoCollection<RiotTFTMatchlistModel>("tft_summoner_matchlist");

            var filter = Builders<RiotTFTMatchlistModel>.Filter
                .Eq(summoner => summoner.Puuid, userMatchlist.Puuid);

            var update = Builders<RiotTFTMatchlistModel>.Update
                .Set(summoner => summoner.Matchlist, userMatchlist.Matchlist);

            UpdateItem(userMatchlist, collection, filter, update);
        }

        public RiotTFTMatchModel GetRiotTFTMatch(string matchId)
        {
            var collection = GetMongoCollection<RiotTFTMatchModel>("tft_match");

            var filter = Builders<RiotTFTMatchModel>.Filter
                .Eq(summoner => summoner.Metadata.Match_id, matchId);

            var existing = collection.Find(filter).FirstOrDefault();

            return existing;
        }
        public void UpdateRiotTFTMatch(RiotTFTMatchModel userMatch)
        {
            var collection = GetMongoCollection<RiotTFTMatchModel>("tft_match");

            var filter = Builders<RiotTFTMatchModel>.Filter
                .Eq(summoner => summoner.Metadata.Match_id, userMatch.Metadata.Match_id);

            UpdateItem(userMatch, collection, filter);
        }

        // TFT Extension with Different Leagues

        //public RiotTFTLeagueEntryModel GetRiotTFTLeagueSummoner(string summonerId)
        //{
        //    var collection = GetMongoCollection<RiotTFTLeagueEntryModel>("tft_league_summoner");

        //    var filter = Builders<RiotTFTLeagueEntryModel>.Filter
        //        .Eq(league => league.SummonerId, summonerId);

        //    var existing = collection.Find(filter).FirstOrDefault();

        //    return existing;
        //}
        public RiotTFTLeagueSummonerAndSummoner? GetRiotTFTLeagueSummoner(string summonerId, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            RiotTFTLeagueSummonerAndSummoner? existing = null;

            try
            {
                MongoClient dbClient = databaseService.InitDatabaseCon();

                var database = dbClient.GetDatabase("tft_table");

                var collection = database.GetCollection<RiotTFTLeagueSummonerModel>("tft_league_summoner");
                var collectionSummoner = database.GetCollection<RiotTFTSummonerModel>("tft_summoner");

                existing = collection.Aggregate()
                .Match(u => u.SummonerId == summonerId)
                .Match(u => u.Region == platform.ToString())
                .Lookup<RiotTFTLeagueSummonerModel, RiotTFTSummonerModel, RiotTFTLeagueSummonerModelWithExtra>(
                    collectionSummoner,
                    x => x.SummonerId,
                    x => x.Id,
                    x => x.Summoners
                )
                .Project(StaticAPIData.CustomLeagueSummonerProjection)
                .FirstOrDefault();
            }
            catch (Exception)
            {}

            return existing;
        }
        public List<RiotTFTLeagueSummonerAndSummoner> GetRiotTFTLeagueEntryList(string? leagueId)
        {
            MongoClient dbClient = databaseService.InitDatabaseCon();

            var database = dbClient.GetDatabase("tft_table");

            var collection = database.GetCollection<RiotTFTLeagueSummonerModel>("tft_league_summoner");
            var collectionSummoner = database.GetCollection<RiotTFTSummonerModel>("tft_summoner");

            var filter = Builders<RiotTFTLeagueSummonerModel>.Filter
                .Eq(summoner => summoner.LeagueId, leagueId);

            var existing = collection.Aggregate()
            .Match(filter)
            .Lookup<RiotTFTLeagueSummonerModel, RiotTFTSummonerModel, RiotTFTLeagueSummonerModelWithExtra>(
                collectionSummoner,
                x => x.SummonerId,
                x => x.Id,
                x => x.Summoners
            )
            .Project(StaticAPIData.CustomLeagueSummonerProjection)
            .ToList();

            return existing;
        }
        public void UpdateRiotTFTLeagueSummoner(RiotTFTLeagueSummonerModel leagueSummoner, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            var collection = GetMongoCollection<RiotTFTLeagueSummonerModel>("tft_league_summoner");

            var filter = Builders<RiotTFTLeagueSummonerModel>.Filter
                .Where(summoner =>
                summoner.SummonerId == leagueSummoner.SummonerId &&
                summoner.LeagueId == leagueSummoner.LeagueId &&
                summoner.Region == platform.ToString()
                );

            UpdateItem(leagueSummoner, collection, filter);
        }

        public RiotTFTLeagueModel? GetRiotTFTLeagueList(RiotTFTLeagueType type = RiotTFTLeagueType.challenger, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            var collection = GetMongoCollection<RiotTFTLeagueModel>("tft_league");

            var filter = Builders<RiotTFTLeagueModel>.Filter
                .Where(league => league.Tier == type.ToString().ToUpper() && league.Region == platform.ToString());

            var existing = collection.Find(filter).FirstOrDefault();

            if (existing != null)
            {
                existing.Entries = GetRiotTFTLeagueEntryList(existing.LeagueId);
            }

            return existing;
        }
        public List<RiotTFTLeagueSummonerAndSummoner>? GetRiotTFTLeagueTop3Mixed(RiotTFTAPIplatformEnum platformOverride = RiotTFTAPIplatformEnum.euw1)
        {
            MongoClient dbClient = databaseService.InitDatabaseCon();

            var database = dbClient.GetDatabase("tft_table");

            var collection = database.GetCollection<RiotTFTLeagueSummonerModel>("tft_league_summoner");
            var collectionSummoner = database.GetCollection<RiotTFTSummonerModel>("tft_summoner");

            List<RiotTFTLeagueSummonerAndSummoner> existing = new List<RiotTFTLeagueSummonerAndSummoner>();

            var filter = Builders<RiotTFTLeagueSummonerModel>.Filter
                .Where(summoner => platformOverride != RiotTFTAPIplatformEnum.none ? summoner.Region == platformOverride.ToString() : true);

            existing.Add(collection.Aggregate()
                .Match(filter)
                .SortByDescending(u => u.LeaguePoints)
                .Lookup<RiotTFTLeagueSummonerModel, RiotTFTSummonerModel, RiotTFTLeagueSummonerModelWithExtra>(
                    collectionSummoner,
                    x => x.SummonerId,
                    x => x.Id,
                    x => x.Summoners
                )
                .Project(StaticAPIData.CustomLeagueSummonerProjection)
                .FirstOrDefault()
            );

            existing.Add(collection.Aggregate()
                .Match(filter)
                .SortByDescending(u => u.Wins)
                .Lookup<RiotTFTLeagueSummonerModel, RiotTFTSummonerModel, RiotTFTLeagueSummonerModelWithExtra>(
                    collectionSummoner,
                    x => x.SummonerId,
                    x => x.Id,
                    x => x.Summoners
                )
                .Project(StaticAPIData.CustomLeagueSummonerProjection)
                .FirstOrDefault()
            );

            existing.Add(collection.Aggregate()
                .Match(filter)
                .SortByDescending(u => u.WinRate)
                .Lookup<RiotTFTLeagueSummonerModel, RiotTFTSummonerModel, RiotTFTLeagueSummonerModelWithExtra>(
                    collectionSummoner,
                    x => x.SummonerId,
                    x => x.Id,
                    x => x.Summoners
                )
                .Project(StaticAPIData.CustomLeagueSummonerProjection)
                .FirstOrDefault()
            );

            List<RiotTFTLeagueSummonerAndSummoner> result = new List<RiotTFTLeagueSummonerAndSummoner>();

            foreach (var item in existing)
            {
                if (item == null)
                {
                    result.Add(new RiotTFTLeagueSummonerAndSummoner() { Summoner = new() { ProfileIconId = 4906} }); // Defaulting
                }
                else
                {
                    if (item.Summoner == null)
                    {
                        item.Summoner = new() { ProfileIconId = 4906 }; // Defaulting
                    }
                    result.Add(item);
                }
            }

            return result;
        }

        public List<RiotTFTLeagueSummonerAndSummoner>? GetRiotTFTLeagueTop3LP(RiotTFTAPIplatformEnum platformOverride = RiotTFTAPIplatformEnum.euw1)
        {
            MongoClient dbClient = databaseService.InitDatabaseCon();

            var database = dbClient.GetDatabase("tft_table");

            var collection = database.GetCollection<RiotTFTLeagueSummonerModel>("tft_league_summoner");
            var collectionSummoner = database.GetCollection<RiotTFTSummonerModel>("tft_summoner");

            var filter = Builders<RiotTFTLeagueSummonerModel>.Filter
                .Where(summoner => platformOverride != RiotTFTAPIplatformEnum.none ? summoner.Region == platformOverride.ToString() : true);

            var temp = collection.Aggregate()
                .Match(filter)
                .SortByDescending(u => u.LeaguePoints)
                .Lookup<RiotTFTLeagueSummonerModel, RiotTFTSummonerModel, RiotTFTLeagueSummonerModelWithExtra>(
                    collectionSummoner,
                    x => x.SummonerId,
                    x => x.Id,
                    x => x.Summoners
                )
                .Project(StaticAPIData.CustomLeagueSummonerProjection)
                .ToList().Take(3);

            List<RiotTFTLeagueSummonerAndSummoner> result = new List<RiotTFTLeagueSummonerAndSummoner>();

            foreach (var item in temp)
            {
                if (item == null)
                {
                    result.Add(new RiotTFTLeagueSummonerAndSummoner() { Summoner = new() { ProfileIconId = 4906 } }); // Defaulting
                }
                else
                {
                    if (item.Summoner == null)
                    {
                        item.Summoner = new() { ProfileIconId = 4906 }; // Defaulting
                    }
                    result.Add(item);
                }
            }

            return result;
        }

        public void UpdateRiotTFTLeagueList(RiotTFTLeagueModel league, RiotTFTLeagueType type = RiotTFTLeagueType.challenger, RiotTFTAPIplatformEnum platform = RiotTFTAPIplatformEnum.euw1)
        {
            if (league != null)
            {
                try
                {
                    if (league.Entries != null)
                    {
                        foreach (var item in league.Entries)
                        {
                            if (item.LeagueSummoner != null)
                            {
                                item.LeagueSummoner.Region = platform.ToString();
                                item.LeagueSummoner.LeagueId = league.LeagueId;
                                item.LeagueSummoner.QueueType = league.Queue;
                                item.LeagueSummoner.Tier = league.Tier;
                                UpdateRiotTFTLeagueSummoner(item.LeagueSummoner);
                            }
                        }
                    }

                    league.Entries = null;

                    var collection = GetMongoCollection<RiotTFTLeagueModel>("tft_league");

                    var filter = Builders<RiotTFTLeagueModel>.Filter
                        .Where(league =>
                            league.Tier != null &&
                            league.Tier == type.ToString().ToUpper() &&
                            league.Region == platform.ToString()
                        );

                    UpdateItem(league, collection, filter);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex?.Message);
                }
            }
        }
    }
}
