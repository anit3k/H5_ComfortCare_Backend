using ComfortCare.Data.Interfaces;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace ComfortCare.Data
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);

            RegisterConventions();
        }

        private void RegisterConventions()
        {
            var conventionPack = new ConventionPack
                {
                    new CamelCaseElementNameConvention(),
                    new IgnoreExtraElementsConvention(true)
                };
            ConventionRegistry.Register("MyConventions", conventionPack, t => true);
        }

        public void Insert<T>(T entity, string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            collection.InsertOne(entity);
        }

        public List<T> GetAll<T>(string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(_ => true).ToList();
        }

        public List<T> Get<T>(Expression<Func<T, bool>> filter, string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(filter).ToList();
        }
    }
}
