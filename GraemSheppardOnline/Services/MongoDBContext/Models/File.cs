using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    public class File : Document
    {
        public string Name { get; set; }

        [BsonIgnoreIfNull]
        public string DisplayName { get; set; }

        [BsonIgnoreIfDefault]
        public bool IsPublic { get; set; }

        [BsonIgnoreIfDefault]
        public bool IsImage { get; set; }


    }
}
