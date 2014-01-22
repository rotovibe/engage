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
        
        public const string SourceIdProperty = "sourceid";
        public const string OrderProperty = "order";
        public const string EnabledProperty = "enabled";
        public const string StateProperty = "state";
        public const string AssignDateProperty = "assigndate";
        public const string AssignByProperty = "assignby";
        public const string CompletedByProperty = "completedby";
        public const string CompletedProperty = "completed";
        public const string DateCompletedProperty = "datecompleted";
        public const string NextProperty = "next";
        public const string PreviousProperty = "previous";
        public const string SpawnProperty = "spawn";

        [BsonElement(SourceIdProperty)]
        [BsonIgnoreIfNull(false)]
        public string SourceId { get; set; }

        [BsonElement(OrderProperty)]
        [BsonIgnoreIfNull(false)]
        public int Order { get; set; }

        [BsonElement(EnabledProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Enabled { get; set; }

        [BsonElement(StateProperty)]
        [BsonIgnoreIfNull(true)]
        public ElementState ElementState { get; set; }

        [BsonElement(AssignDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? AssignDate { get; set; }

        [BsonElement(AssignByProperty)]
        [BsonIgnoreIfNull(true)]
        public string AssignBy { get; set; }

        [BsonElement(CompletedByProperty)]
        [BsonIgnoreIfNull(true)]
        public string CompletedBy { get; set; }

        [BsonElement(CompletedProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Completed { get; set; }

        [BsonElement(DateCompletedProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? DateCompleted { get; set; }

        [BsonElement(NextProperty)]
        [BsonIgnoreIfNull(true)]
        public string Next { get; set; }

        [BsonElement(PreviousProperty)]
        [BsonIgnoreIfNull(true)]
        public string Previous { get; set; }

        [BsonElement(SpawnProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MESpawnElement> Spawn { get; set; }
    }

    public class MESpawnElement
    {
        public MESpawnElement() { }

        public const string TypeProperty = "type";
        public const string SpawnIdProperty = "spawnid";

        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public int Type { get; set; }

        [BsonElement(SpawnIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId SpawnId { get; set; }
    }
}