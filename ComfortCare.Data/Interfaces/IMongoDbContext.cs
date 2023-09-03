using System.Linq.Expressions;

namespace ComfortCare.Data.Interfaces
{
    public interface IMongoDbContext
    {
        public void Insert<T>(T entity, string collectionName);
        public IEnumerable<T> GetAll<T>(string collectionName);
        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> filter, string collectionName);
    }
}
