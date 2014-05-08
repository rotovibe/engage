using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty, }, TimeToLive = 0)]
    public class MEStep : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEStep(string userId) 
        { 
            Id = ObjectId.GenerateNewId();
            RecordCreatedBy = ObjectId.Parse(userId);
            RecordCreatedOn = System.DateTime.UtcNow;
            Version = 1.0;
        }

        public const string IdProperty = "_id";
        public const string ActionIdProperty = "actid";
        public const string TypeProperty = "sttid";
        public const string HeaderProperty = "hdr";
        public const string SelectedResponseIdProperty = "srid";
        public const string ControlTypeProperty = "ctype";
        public const string SelectTypeProperty = "selt";
        public const string IncludeTimeProperty = "it";
        public const string QuestionProperty = "q";
        public const string TProperty = "t";
        public const string DescriptionProperty = "desc";
        public const string NotesProperty = "nts";
        public const string TextProperty = "txt";
        public const string StatusProperty = "st";

        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        public const string RecordCreatedByProperty = "rcby";
        public const string RecordCreatedOnProperty = "rcon";


        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedByProperty)]
        public ObjectId RecordCreatedBy { get; private set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement(RecordCreatedOnProperty)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime RecordCreatedOn { get; private set; }

        [BsonElement(IdProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(ActionIdProperty)]
        public ObjectId ActionId { get; set; }

        [BsonElement(TypeProperty)]
        public int StepTypeId { get; set; }

        [BsonElement(HeaderProperty)]
        public string Header { get; set; }

        [BsonElement(SelectedResponseIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SelectedResponseId { get; set; }

        [BsonElement(ControlTypeProperty)]
        public ControlType ControlType { get; set; }

        [BsonElement(SelectTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public SelectType SelectType { get; set; }

        [BsonElement(IncludeTimeProperty)]
        [BsonIgnoreIfNull(true)]
        public bool IncludeTime { get; set; }

        [BsonElement(QuestionProperty)]
        public string Question { get; set; }

        [BsonElement(TProperty)]
        [BsonIgnoreIfNull(true)]
        public string Title { get; set; }

        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        [BsonElement(NotesProperty)]
        [BsonIgnoreIfNull(true)]
        public string Notes { get; set; }

        [BsonElement(TextProperty)]
        public string Text { get; set; }

        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDefaultValue(1.0)]
        public Status Status { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        public ObjectId? UpdatedBy { get; set; }

        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        public System.DateTime? LastUpdatedOn { get; set; }

        //public const string ResponsesProperty = "resp";
        //[BsonElement(ResponsesProperty)]
        //public List<MEPatientProgramResponse> Responses { get; set; }
    }
}
