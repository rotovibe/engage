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
    public class Step : PlanElement
    {
        public Step() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        [BsonElement(IdProperty)]
        public ObjectId Id { get; set; }

        public const string ActionIdProperty = "actid";
        [BsonElement(ActionIdProperty)]
        public ObjectId ActionId { get; set; }

        public const string TypeProperty = "sttid";
        [BsonElement(TypeProperty)]
        public int StepTypeId { get; set; }

        public const string HeaderProperty = "hdr";
        [BsonElement(HeaderProperty)]
        public string Header { get; set; }

        public const string SelectedResponseIdProperty = "srid";
        [BsonElement(SelectedResponseIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SelectedResponseId { get; set; }

        public const string ControlTypeProperty = "ctype";
        [BsonElement(ControlTypeProperty)]
        public ControlType ControlType { get; set; }

        public const string SelectTypeProperty = "selt";
        [BsonElement(SelectTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public SelectType SelectType { get; set; }

        public const string IncludeTimeProperty = "it";
        [BsonElement(IncludeTimeProperty)]
        [BsonIgnoreIfNull(true)]
        public bool IncludeTime { get; set; }

        public const string QuestionProperty = "q";
        [BsonElement(QuestionProperty)]
        public string Question { get; set; }

        public const string TProperty = "t";
        [BsonElement(TProperty)]
        [BsonIgnoreIfNull(true)]
        public string Title { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        public string Description { get; set; }

        public const string NotesProperty = "nts";
        [BsonElement(NotesProperty)]
        [BsonIgnoreIfNull(true)]
        public string Notes { get; set; }

        public const string TextProperty = "txt";
        [BsonElement(TextProperty)]
        public string Text { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }

        public const string ResponsesProperty = "resp";
        [BsonElement(ResponsesProperty)]
        public List<MEPatientProgramResponse> Responses { get; set; }
    }
}
