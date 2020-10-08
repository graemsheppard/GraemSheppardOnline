using MongoDB.Bson.Serialization.Attributes;

namespace GraemSheppardOnline.Services.MongoDBContext
{

    [BsonIgnoreExtraElements]
    public class Authenticate : Document
    {
        public string Password { get; set; }
    }
}
