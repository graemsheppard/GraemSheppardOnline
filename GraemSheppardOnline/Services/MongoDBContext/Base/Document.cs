using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace GraemSheppardOnline.Services.MongoDBContext
{

    public class Document
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
