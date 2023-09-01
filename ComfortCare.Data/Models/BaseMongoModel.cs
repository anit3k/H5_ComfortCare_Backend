using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ComfortCare.Data.Models
{
    public abstract class BaseMongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
