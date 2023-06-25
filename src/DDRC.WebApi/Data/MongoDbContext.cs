using DDRC.WebApi.Models;
using MongoDB.Driver;

namespace DDRC.WebApi.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Mongo");

            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            var mongoSettings = MongoClientSettings.FromConnectionString(connectionString);
            var mongoClient = new MongoClient(mongoSettings);

            _database = mongoClient.GetDatabase("DDRC");
        }

        public IMongoCollection<CategoryModel> Categories
        {
            get
            {
                return _database.GetCollection<CategoryModel>("Categories");
            }
        }

        public IMongoCollection<ActorModel> Actors
        {
            get
            {
                return _database.GetCollection<ActorModel>("Actors");
            }
        }

        public IMongoCollection<MovieModel> Movies
        {
            get
            {
                return _database.GetCollection<MovieModel>("Movies");
            }
        }

        public IMongoCollection<VideoStoreModel> VideoStores
        {
            get
            {
                return _database.GetCollection<VideoStoreModel>("VideoStores");
            }
        }

        public IMongoCollection<FulfilledSaleModel> FulfilledSales
        {
            get
            {
                return _database.GetCollection<FulfilledSaleModel>("FulfilledSales");
            }
        }

        public IMongoCollection<ExpectedSaleModel> ExpectedSales
        {
            get
            {
                return _database.GetCollection<ExpectedSaleModel>("ExpectedSales");
            }
        }

        public IMongoCollection<StockModel> Stocks
        {
            get
            {
                return _database.GetCollection<StockModel>("Stocks");
            }
        }
    }
}
