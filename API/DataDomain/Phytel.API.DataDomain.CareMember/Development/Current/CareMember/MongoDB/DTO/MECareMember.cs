using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.CareMember.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MECareMember : IMongoEntity<ObjectId>, IMEEntity
    {
        public MECareMember() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string PatientIdProperty = "pid";
        public const string ContactIdProperty = "cid";
        public const string PrimaryProperty = "prim";
        public const string TypeProperty = "type";

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

        [BsonElement(ContactIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId ContactId { get; set; }

        [BsonElement(PrimaryProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Primary { get; set; }

        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(false)]
        public Object Type { get; set; }

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
