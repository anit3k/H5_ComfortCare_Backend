using ComfortCare.Data.Interfaces;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

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
    }
}
