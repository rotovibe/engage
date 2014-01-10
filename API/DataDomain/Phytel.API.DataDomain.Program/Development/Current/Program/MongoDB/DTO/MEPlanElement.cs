using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    public class MEPlanElement
    {
        public MEPlanElement() { }

        public const string OrderProperty = "order";
        [BsonElement(OrderProperty)]
        [BsonIgnoreIfNull(false)]
        public int Order { get; set; }

        public const string EnabledProperty = "enabled";
        [BsonElement(EnabledProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Enabled { get; set; }

        public const string CompletedProperty = "completed";
        [BsonElement(CompletedProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Completed { get; set; }

        public const string NextProperty = "next";
        [BsonElement(NextProperty)]
        [BsonIgnoreIfNull(true)]
        public string Next { get; set; }

        public const string PreviousProperty = "previous";
        [BsonElement(PreviousProperty)]
        [BsonIgnoreIfNull(true)]
        public string Previous { get; set; }

        public const string SpawnProperty = "spawn";
        [BsonElement(SpawnProperty)]
        [BsonIgnoreIfNull(true)]
        public MESpawnElement Spawn { get; set; }
    }

    public class MESpawnElement
    {
        public MESpawnElement() { }

        public const string TypeProperty = "type";
        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public int Type { get; set; }

        public const string SpawnIdProperty = "spawnid";
        [BsonElement(SpawnIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId SpawnId { get; set; }
    }
}