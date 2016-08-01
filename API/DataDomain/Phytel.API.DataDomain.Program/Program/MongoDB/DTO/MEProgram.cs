using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { StatusProperty }, Unique = false)]
    public class MEProgram : ProgramBase, IMEEntity, IMongoEntity<ObjectId>
    {
        public MEProgram(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = DateTime.UtcNow;
        }

        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; set; }

        public const string ProgramTemplateIdProperty = "ptid";
        [BsonElement(ProgramTemplateIdProperty)]
        [BsonIgnoreIfNull(false)]
        public ObjectId ProgramTemplateId { get; set; }

        public const string PopulationProperty = "pop";
        [BsonElement(PopulationProperty)]
        [BsonIgnoreIfNull(false)]
        public string Population { get; set; }

        public const string ContractIdProperty = "ctid";
        [BsonElement(ContractIdProperty)]
        [BsonIgnoreIfNull(false)]
        public string ContractId { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonDefaultValue(null)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(LastUpdatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime? LastUpdatedOn { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }

        public const string AssignToTypeProperty = "atotyp";
        [BsonElement(AssignToTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public AssignToType AssignToType { get; set; }
    }
}
