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
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEContractProgram : ProgramBase, IMEEntity, IMongoEntity<ObjectId>
    {
        public MEContractProgram(){ Id = ObjectId.GenerateNewId(); }

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

        public const string ContractIdProperty = "cid";
        [BsonElement(ContractIdProperty)]
        [BsonIgnoreIfNull(false)]
        public string ContractId { get; set; }

        public const string ExtraElementsProperty = "ex";
        [BsonExtraElements]
        [BsonIgnoreIfNull(true)]
        [BsonElement(ExtraElementsProperty)]
        public Dictionary<string, object> ExtraElements { get; set; }

        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        [BsonIgnoreIfNull(true)]
        public string UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonDefaultValue(null)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(LastUpdatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? LastUpdatedOn { get; set; }
    }
}
