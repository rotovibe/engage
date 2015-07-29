using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.Services.Migrations
{
    public class Database
    {
        [BsonElement(FieldNames.Name)]
        public string Name { get; set; }

        [BsonElement(FieldNames.Server)]
        public string Server { get; set; }

        [BsonElement(FieldNames.Type)]
        public DatabaseTypes Type { get; set; }

        public static class FieldNames
        {
            public const string Name = "name";
            public const string Server = "server";
            public const string Type = "type";
        }
    }
}