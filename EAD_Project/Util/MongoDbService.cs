using MongoDB.Driver;

namespace EAD_Project.Util
{
    public class MongoDbService
    {
        private readonly IConfiguration _config;
        private readonly IMongoDatabase? _database;

        public MongoDbService(IConfiguration config)
        {
            _config = config;
            var connectionString = _config.GetConnectionString("DbConnection");
            var database = _config.GetConnectionString("Database");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(database);
        }

        public IMongoDatabase? Database => _database;
    }
}
