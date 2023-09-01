using ComfortCare.Data.Interfaces;
using ComfortCare.Data.Models;
using MongoDB.Bson;
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

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public void Insert<T>(T entity, string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            collection.InsertOne(entity);
        }

        public void Update<T>(T entity, string collectionName) where T : BaseMongoModel
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", ObjectId.Parse(entity.Id.ToString()));
            collection.ReplaceOne(filter, entity);
        }

        public void Delete<T>(Expression<Func<T, bool>> filter, string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            collection.DeleteMany(filter);
        }

        public IEnumerable<T> GetAll<T>(string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(_ => true).ToList();
        }

        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> filter, string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return collection.Find(filter).ToList();
        }
    }
}
