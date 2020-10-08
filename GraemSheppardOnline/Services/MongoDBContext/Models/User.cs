using MongoDB.Bson.Serialization.Attributes;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    [BsonIgnoreExtraElements]
    public class User : Document
    {
        [BsonIgnoreIfNull]
        public string AuthToken { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        [BsonIgnoreIfNull]
        public string FirstName { get; set; }
        [BsonIgnoreIfNull]
        public string LastName { get; set; }
        [BsonIgnoreIfNull]
        public string Role { get; set; }
        


    }
}
