using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    [BsonIgnoreExtraElements]
    public class Item : Document
    {
        [BsonIgnoreIfNull]
        public string PriceId { get; set; }
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ParentId { get; set; }
        [BsonIgnoreIfNull]
        public string Name { get; set; }
        [BsonIgnoreIfNull]
        public Nested Nested { get; set; }
        [BsonIgnoreIfNull]
        public List<string> Alphabet { get; set; }

    }

    [BsonIgnoreExtraElements]
    public class Nested
    {
        [BsonIgnoreIfNull]
        public string One { get; set; }
        [BsonIgnoreIfNull]
        public string Two { get; set; }
    }
}
