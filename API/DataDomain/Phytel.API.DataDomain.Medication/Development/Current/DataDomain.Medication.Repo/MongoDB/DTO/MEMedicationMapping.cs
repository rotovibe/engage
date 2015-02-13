using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Medication.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] { FullNameProperty, DeleteFlagProperty, TTLDateProperty }, Unique = false)]
    public class MEMedicationMapping : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEMedicationMapping()
        {
            Id = ObjectId.GenerateNewId();
        }

        public MEMedicationMapping(string userId)
        {
            Id = ObjectId.GenerateNewId();
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
            Version = 1.0;
        }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; private set; }

        public const string SubstanceNameProperty = "sbsnm";
        [BsonElement(SubstanceNameProperty)]
        public string SubstanceName { get; set; }

        /// <summary>
        /// Proprietray Name + proprietary name suffix
        /// </summary>
        public const string FullNameProperty = "fnm";
        [BsonElement(FullNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string FullName { get; set; }

        public const string CustomProperty = "cust";
        [BsonElement(CustomProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Custom { get; set; }

        public const string VerifiedProperty = "vrfy";
        [BsonElement(VerifiedProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Verified { get; set; }

        public const string StrengthProperty = "str";
        [BsonElement(StrengthProperty)]
        public string Strength { get; set; }

        public const string RouteProperty = "route";
        [BsonElement(RouteProperty)]
        public string Route { get; set; }

        public const string FormProperty = "frm";
        [BsonElement(FormProperty)]
        public string Form { get; set; }

        #region Base elements
        public const string VersionProperty = "v";
        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        public const string UpdatedByProperty = "uby";
        [BsonElement(UpdatedByProperty)]
        public ObjectId? UpdatedBy { get; set; }

        public const string DeleteFlagProperty = "del";
        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        public const string TTLDateProperty = "ttl";
        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? LastUpdatedOn { get; set; }

        public const string RecordCreatedByProperty = "rcby";
        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; set; }

        public const string RecordCreatedOnProperty = "rcon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; set; }

        public const string ExtraElementsProperty = "ex";
        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }
        #endregion
    }
}
