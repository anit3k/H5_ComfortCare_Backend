using ComfortCare.Data.Interfaces;
using ComfortCare.Data.Models;
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
            ConventionRegistry.Register("MyConventions", conventionPack, MB => true);
        }

        public void Insert<MB>(MB entity, string collectionName) where MB : MongoBaseModel
        {
            var collection = _database.GetCollection<MB>(collectionName);
            collection.InsertOne(entity);
        }

        public List<MB> GetAll<MB>(string collectionName) where MB : MongoBaseModel
        {
            var collection = _database.GetCollection<MB>(collectionName);
            return collection.Find(_ => true).ToList();
        }

        public List<MB> Get<MB>(Expression<Func<MB, bool>> filter, string collectionName) where MB : MongoBaseModel
        {
            var collection = _database.GetCollection<MB>(collectionName);
            return collection.Find(filter).ToList();
        }
    }
}
