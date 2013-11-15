using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using Phytel.API.Interface;
using System.Collections.Generic;
using System;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    [MongoIndex(Keys = new string[] { IdProperty })]
    public class MECohort : IMongoEntity<ObjectId>, IMEEntity
    {
        public MECohort() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string NameProperty = "n";
        public const string ShortNameProperty = "sn";
        public const string DescriptionProperty = "desc";
        public const string QueryProperty = "q";

        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";

        [BsonId]
        [BsonElement(IdProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        [BsonElement(ShortNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string ShortName { get; set; }

        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        [BsonElement(QueryProperty)]
        [BsonIgnoreIfNull(true)]
        public string Query { get; set; }

        public Dictionary<string, object> ExtraElements { get; set; }
       
        public string Version { get; set; } 

        public string UpdatedBy { get; set; }

        public bool DeleteFlag { get; set; }

        public DateTime TTLDate { get; set; }

        public DateTime LastUpdatedOn { get; set; }
    }
}
