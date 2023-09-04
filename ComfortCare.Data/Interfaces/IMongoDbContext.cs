using ComfortCare.Data.Models;
using System.Linq.Expressions;

namespace ComfortCare.Data.Interfaces
{
    public interface IMongoDbContext
    {
        public void Insert<MB>(MB entity, string collectionName) where MB : MongoBaseModel;
        public List<MB> GetAll<MB>(string collectionName) where MB : MongoBaseModel;
        public List<MB> Get<MB>(Expression<Func<MB, bool>> filter, string collectionName) where MB : MongoBaseModel;
    }
}
