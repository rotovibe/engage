﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Medication.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEPatientMedSupp : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientMedSupp(string userId)
        {
            Id = ObjectId.GenerateNewId();
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
            Version = 1.0;
        }

        public const string IdProperty = "_id";
        [BsonId]
        public ObjectId Id { get; private set; }

        public const string PatientIdProperty = "pid";
        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        public const string FamilyIdProperty = "fmid";
        [BsonElement(FamilyIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? FamilyId { get; set; }

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        public const string CategoryProperty = "cat";
        [BsonElement(CategoryProperty)]
        [BsonIgnoreIfNull(true)]
        public Category CategoryId  { get; set; }

        public const string TypeIdProperty = "typeid";
        [BsonElement(TypeIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId TypeId { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status StatusId { get; set; }

        public const string DosageProperty = "dsg";
        [BsonElement(DosageProperty)]
        [BsonIgnoreIfNull(true)]
        public string Dosage { get; set; }

        public const string StrengthProperty = "str";
        [BsonElement(StrengthProperty)]
        public string Strength { get; set; }

        public const string RouteProperty = "route";
        [BsonElement(RouteProperty)]
        public string Route { get; set; }

        public const string FormProperty = "frm";
        [BsonElement(FormProperty)]
        public string Form { get; set; }

        public const string PharmClassProperty = "phcls";
        [BsonElement(PharmClassProperty)]
        public List<string> PharmClasses { get; set; }

        public const string NDCProperty = "ndc";
        [BsonElement(NDCProperty)]
        public List<string> NDCs { get; set; }

        public const string FreqQuantityProperty = "quant";
        [BsonElement(FreqQuantityProperty)]
        [BsonIgnoreIfNull(true)]
        public string FreqQuantity { get; set; }

        public const string FreqHowOftenIdProperty = "hoid";
        [BsonElement(FreqHowOftenIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? FreqHowOftenId { get; set; }

        public const string FreqWhenIdProperty = "whenid";
        [BsonElement(FreqWhenIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? FreqWhenId { get; set; }

        public const string SourceIdProperty = "srcid";
        [BsonElement(SourceIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId SourceId { get; set; }

        public const string StartDateProperty = "sd";
        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? StartDate { get; set; }

        public const string EndDateProperty = "ed";
        [BsonElement(EndDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EndDate { get; set; }

        public const string ReasonProperty = "rsn";
        [BsonElement(ReasonProperty)]
        [BsonIgnoreIfNull(true)]
        public string Reason { get; set; }

        public const string NotesProperty = "nts";
        [BsonElement(NotesProperty)]
        [BsonIgnoreIfNull(true)]
        public string Notes { get; set; }

        public const string PrescribedByProperty = "pby";
        [BsonElement(PrescribedByProperty)]
        [BsonIgnoreIfNull(true)]
        public string PrescribedBy { get; set; }

        public const string SystemProperty = "sys";
        [BsonElement(SystemProperty)]
        [BsonIgnoreIfNull(true)]
        public string SystemName { get; set; }

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
