using H3WebAPI.Models.RiotModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace H3WebAPI.Services
{
    public class DatabaseService
    {
        private DateTime lastCheckTime = DateTime.Now.AddDays(-1);

        public DatabaseService()
        {
            InitMapping(); // Init MongoDB Class Mapping (Called only once since this Service is used as a Singleton)
        }

        /// <summary>
        /// Crude Call limiting function
        /// </summary>
        public bool CanBeCalled()
        {
            if (lastCheckTime.AddSeconds(2) < DateTime.Now)
            {
                lastCheckTime = DateTime.Now;
                return true;
            }

            return false;
        }

        public MongoClient InitDatabaseCon()
        {
            MongoClient dbClient = new MongoClient("mongodb://localhost:27017");

            return dbClient;
        }

        /// <summary>
        /// Not necessary but nice to have for setting up new classes with more custom setups
        /// </summary>
        public void InitMapping()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(RiotAccountModel)))
            {
                BsonClassMap.RegisterClassMap<RiotAccountModel>();
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(RiotTFTSummonerModel)))
            {
                BsonClassMap.RegisterClassMap<RiotTFTSummonerModel>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapIdMember(c => c.UniqueId).SetSerializer(new StringSerializer(BsonType.ObjectId)).SetIgnoreIfDefault(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(RiotTFTMatchlistModel)))
            {
                BsonClassMap.RegisterClassMap<RiotTFTMatchlistModel>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapIdMember(c => c.UniqueId).SetSerializer(new StringSerializer(BsonType.ObjectId)).SetIgnoreIfDefault(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(RiotTFTMatchModel)))
            {
                BsonClassMap.RegisterClassMap<RiotTFTMatchModel>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapIdMember(c => c.UniqueId).SetSerializer(new StringSerializer(BsonType.ObjectId)).SetIgnoreIfDefault(true);
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(RiotTFTLeagueModel)))
            {
                BsonClassMap.RegisterClassMap<RiotTFTLeagueModel>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapIdMember(c => c.UniqueId).SetSerializer(new StringSerializer(BsonType.ObjectId)).SetIgnoreIfDefault(true);
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(RiotTFTLeagueSummonerModel)))
            {
                BsonClassMap.RegisterClassMap<RiotTFTLeagueSummonerModel>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                    cm.MapIdMember(c => c.UniqueId).SetSerializer(new StringSerializer(BsonType.ObjectId)).SetIgnoreIfDefault(true);
                });
            }
        }
    }
}
