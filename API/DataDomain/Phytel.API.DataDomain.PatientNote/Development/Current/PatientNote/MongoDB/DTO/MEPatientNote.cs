using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEPatientNote : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientNote() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string PatientIdProperty = "pid";
        public const string TextProperty = "txt";
        public const string ProgramProperty = "prog";
        public const string CreatedOnProperty = "con";
        public const string CreatedByProperty = "cby";
        

        #region Standard IMongoEntity Constants
        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        #endregion

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        [BsonElement(TextProperty)]
        [BsonIgnoreIfNull(true)]
        public string Text { get; set; }

        [BsonElement(ProgramProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> Programs { get; set; }

        [BsonElement(CreatedByProperty)]
        [BsonIgnoreIfNull(false)]
        public string CreatedBy { get; set; }

        [BsonElement(CreatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime CreatedOn { get; set; }

        #region Standard IMongoEntity Implementation
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
        #endregion
    }
}
