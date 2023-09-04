using ComfortCare.Data.Models;
using System.Linq.Expressions;

namespace ComfortCare.Data.Interfaces
{
    public interface IMongoDbContext
    {
        public void Insert<T>(T entity, string collectionName) where T: MongoBaseModel;
        public List<T> GetAll<T>(string collectionName) where T: MongoBaseModel;
        public List<T> Get<T>(Expression<Func<T, bool>> filter, string collectionName) where T: MongoBaseModel;
    }
}
