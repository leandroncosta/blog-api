using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using MongoDB.Driver;
using Moq;

namespace api.Tests.Services
{
    public class MongoDbServiceTests
    {
     
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly Mock<MongoClient> _mockClient;
      


        public MongoDbServiceTests()
        {
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockClient = new Mock<MongoClient>();

        }

    }
}


//using MongoDB.Driver;

//namespace api.Services
//{
//    public class MongoDbService
//    {
//        private readonly IMongoDatabase _database;

//        public MongoDbService(IConfiguration configuration)
//        {
//            var conn = configuration["MongoDB:ConnectionString"];
//            var databaseName = configuration["MongoDB:DatabaseName"];

//            var client = new MongoClient(conn);
//            _database = client.GetDatabase(databaseName);
//        }

//        public IMongoCollection<T> GetCollection<T>(string collectionName)
//        {
//            return _database.GetCollection<T>(collectionName);
//        }



//    }
//}
