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
    public class MEMedication : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEMedication()
        {
            Id = ObjectId.GenerateNewId();
        }

        public MEMedication(string userId)
        {
            Id = ObjectId.GenerateNewId();
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
            Version = 1.0;
        }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; private set; }

        public const string ProductIdProperty = "pid";
        [BsonElement(ProductIdProperty)]
        public string ProductId { get; set; } 

        public const string NDCProperty = "ndc";
        [BsonElement(NDCProperty)]
        public string NDC { get; set; }

        public const string FullNameProperty = "flnm";
        [BsonElement(FullNameProperty)]
        public string FullName { get; set; }

        public const string ProprietaryNameProperty = "pnm";
        [BsonElement(ProprietaryNameProperty)]
        public string ProprietaryName { get; set; }

        public const string ProprietaryNameSuffixProperty = "pnmsfx";
        [BsonElement(ProprietaryNameSuffixProperty)]
        public string ProprietaryNameSuffix { get; set; }

        public const string StartDateProperty = "sd";
        [BsonElement(StartDateProperty)]
        public DateTime? StartDate { get; set; }

        public const string EndDateProperty = "ed";
        [BsonElement(EndDateProperty)]
        public DateTime? EndDate { get; set; }

        public const string SubstanceNameProperty = "sbsnm";
        [BsonElement(SubstanceNameProperty)]
        public string SubstanceName { get; set; }

        public const string PharmClassProperty = "phcls";
        [BsonElement(PharmClassProperty)]
        public List<string> PharmClass { get; set; }

        public const string RouteProperty = "route";
        [BsonElement(RouteProperty)]
        public string Route { get; set; }

        public const string FormProperty = "frm";
        [BsonElement(FormProperty)]
        public string Form { get; set; }

        public const string FamilyIdProperty = "fmid";
        [BsonElement(FamilyIdProperty)]
        public ObjectId? FamilyId { get; set; }

        public const string UnitProperty = "unit";
        [BsonElement(UnitProperty)]
        public string Unit { get; set; }

        public const string StrengthProperty = "str";
        [BsonElement(StrengthProperty)]
        public string Strength { get; set; }

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
        public ObjectId RecordCreatedBy { get; private set; }

        public const string RecordCreatedOnProperty = "rcon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }

        public const string ExtraElementsProperty = "ex";
        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }
        #endregion
    }
}
