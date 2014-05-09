using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
//using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataAudit
{
    [Serializable]
    public class DataAudit
    {
        public const string UserIDProperty = "uid";
        public const string TypeProperty = "type";
        public const string EntityTypeProperty = "et";
        public const string EntityIDProperty = "eid";
        public const string EntityProperty = "ey";
        public const string TimeStampProperty = "ts";
        public const string ContractProperty = "ct";

        [BsonElement(UserIDProperty)]
        public string UserId { get; set; }

        [BsonElement(TypeProperty)]
        public string Type { get; set; }  //Insert, Update, Delete

        [BsonElement(EntityTypeProperty)]
        public string EntityType { get; set; }

        [BsonElement(EntityIDProperty)]
        public string EntityID { get; set; }

        [BsonElement(EntityProperty)]
        public string Entity { get; set; }

        [BsonElement(TimeStampProperty)]
        public DateTime TimeStamp { get; set; }

        [BsonElement(ContractProperty)]
        public string Contract { get; set; }
    }
}
