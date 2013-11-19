using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.Collections.Generic;
using System;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { PatientIDProperty })]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive=0)]
    public class MEPatientProblem : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientProblem() { Id = ObjectId.GenerateNewId(); }                                                                                                                                                                                                           

        public const string IdProperty = "_id";
        public const string PatientIDProperty = "pid";
        public const string ProblemIDProperty = "prbid";
        public const string ActiveProperty = "a";
        public const string FeaturedProperty = "f";
        public const string LevelProperty = "l";
        public const string StartDateProperty = "sd";
        public const string EndDateProperty = "ed";

        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIDProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientID { get; set; }

        [BsonElement(ProblemIDProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ProblemID { get; set; }

        [BsonElement(ActiveProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(true)]
        public bool Active { get; set; }

        [BsonElement(FeaturedProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(false)]
        public bool Featured { get; set; }

        [BsonElement(LevelProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(1)]
        public int Level { get; set; }

        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? StartDate { get; set; }

        [BsonElement(EndDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? EndDate { get; set; }

        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements()]
        [BsonIgnoreIfNull(true)]
        public Dictionary<string, object> ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        [BsonDefaultValue("-100")]
        public string UpdatedBy { get; set; }

        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? LastUpdatedOn { get; set; }

    }
}
