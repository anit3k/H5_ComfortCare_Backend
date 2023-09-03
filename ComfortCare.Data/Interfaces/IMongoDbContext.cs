using ComfortCare.Data.Models;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace ComfortCare.Data.Interfaces
{
    public interface IMongoDbContext
    {
        //IMongoCollection<T> GetCollection<T>(string collectionName);
        void Insert<T>(T entity, string collectionName);
        //void Update<T>(T entity, string collectionName) where T : MongoBaseModel;
        //void Delete<T>(Expression<Func<T, bool>> filter, string collectionName);
        List<T> GetAll<T>(string collectionName);
        List<T> Get<T>(Expression<Func<T, bool>> filter, string collectionName);
    }
}
