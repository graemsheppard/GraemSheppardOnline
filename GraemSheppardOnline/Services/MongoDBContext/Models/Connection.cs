using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace GraemSheppardOnline.Services.MongoDBContext
{
    [BsonIgnoreExtraElements]
    public class Connection : Document
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? TimeUTC { get; set; }
        [BsonIgnoreIfNull]
        public string Ip { get; set; }
        [BsonIgnoreIfNull]
        public string HostName { get; set; }
        [BsonIgnoreIfNull]
        public string Type { get; set; }
        [BsonIgnoreIfNull]
        public string ContinentCode { get; set; }
        [BsonIgnoreIfNull]
        public string ContinentName { get; set; }
        [BsonIgnoreIfNull]
        public string CountryCode { get; set; }
        [BsonIgnoreIfNull]
        public string CountryName { get; set; }
        [BsonIgnoreIfNull]
        public string RegionCode { get; set; }
        [BsonIgnoreIfNull]
        public string RegionName { get; set; }
        [BsonIgnoreIfNull]
        public string City { get; set; }
        [BsonIgnoreIfNull]
        public string Zip { get; set; }
        [BsonIgnoreIfNull]
        public double? Latitude { get; set; }
        [BsonIgnoreIfNull]
        public double? Longitude { get; set; }
    }
}
