using MongoDB.Driver;

namespace api.Data
{
    public class MongoDbService : IMongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var conn = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"];

            var client = new MongoClient(conn);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }



    }
}
