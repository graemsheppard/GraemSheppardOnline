using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GraemSheppardOnline.Services.MongoDBContext
{

    [BsonIgnoreExtraElements]
    public class Article : Document
    {
        public int Order { get; set; }
        public string Page { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        
        [BsonIgnoreIfNull]
        public DateTime? LastEdited { get; set; }
    }
}
