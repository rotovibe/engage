using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] {TTLDateProperty}, TimeToLive = 0)]
    [MongoIndex(Keys = new string[] {PatientIdProperty, DeleteFlagProperty}, Unique = false)]
    public class MEPatientUtilization : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEPatientUtilization(string userId)
        {
            Id = ObjectId.GenerateNewId();
            Version = 1.0;
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = DateTime.UtcNow;
        }

        public const string IdProperty = "_id";

        [BsonId]
        public ObjectId Id { get; set; }

        public const string PatientIdProperty = "pid";
        [BsonElement(PatientIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId PatientId { get; set; }

        public const string VisitTypeProperty = "vtp";
        [BsonElement(VisitTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? VisitType { get; set; }


        public const string OtherTypeProperty = "othp";
        [BsonElement(OtherTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public string OtherType { get; set; }

        public const string AdmittedProperty = "admit";
        [BsonElement(AdmittedProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Admitted { get; set; }

        public const string AdmitDateProperty = "admtdt";
        [BsonElement(AdmitDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? AdmitDate { get; set; }

        public const string DischargeDateProperty = "disdt";
        [BsonElement(DischargeDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? DischargeDate { get; set; }

        public const string LocationProperty = "loc";
        [BsonElement(LocationProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? Location { get; set; }

        public const string NoteTypeProperty = "notetyp";
        [BsonElement(NoteTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? NoteType { get; set; }

        public const string OtherLocationProperty = "oloc";
        [BsonElement(OtherLocationProperty)]
        [BsonIgnoreIfNull(true)]
        public string OtherLocation { get; set; }

        public const string ReasonProperty = "rsn";
        [BsonElement(ReasonProperty)]
        [BsonIgnoreIfNull(true)]
        public string Reason { get; set; }

        public const string DispositionProperty = "disp";
        [BsonElement(DispositionProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? Disposition { get; set; }

        public const string OtherDispositionProperty = "odisp";
        [BsonElement(OtherDispositionProperty)]
        [BsonIgnoreIfNull(true)]
        public string OtherDisposition { get; set; }

        public const string SourceIdProperty = "src";
        [BsonElement(SourceIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SourceId { get; set; }

        public const string ProgramsProperty = "progs";
        [BsonElement(ProgramsProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> ProgramIds { get; set; }

        //public const string SystemProperty = "sys";
        //[BsonElement(SystemProperty)]
        //[BsonIgnoreIfNull(true)]
        //public string PSystem { get; set; }

        public const string DataSourceProperty = "dsrc";
        [BsonElement(DataSourceProperty)]
        [BsonIgnoreIfNull(true)]
        public string DataSource { get; set; }

        #region Standard IMongoEntity Implementation
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

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
        public DateTime? TTLDate { get; set; }

        public const string LastUpdatedOnProperty = "uon";
        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime? LastUpdatedOn { get; set; }

        public const string RecordCreatedByProperty = "rcby";
        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        public const string RecordCreatedOnProperty = "rcon";
        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public DateTime RecordCreatedOn { get; private set; }
        #endregion
    }
}
