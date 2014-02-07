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
    public class Step : PlanElement
    {
        public Step() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";
        public const string ActionIdProperty = "actid";
        public const string HeaderProperty = "hdr";
        public const string QuestionProperty = "q";
        public const string ExProperty = "ex";

        [BsonElement(IdProperty)]
        public ObjectId Id { get; set; }

        [BsonElement(ActionIdProperty)]
        public ObjectId ActionId { get; set; }

        public const string TypeProperty = "type";
        [BsonElement(TypeProperty)]
        public int StepTypeId { get; set; }

        [BsonElement(HeaderProperty)]
        public string Header { get; set; }

        public const string SelectedResponseIdProperty = "srid";
        [BsonElement(SelectedResponseIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string SelectedResponseId { get; set; }

        public const string ControlTypeProperty = "ctype";
        [BsonElement(ControlTypeProperty)]
        public int ControlType { get; set; }

        public const string SelectTypeProperty = "selt";
        [BsonElement(SelectTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public int SelectType { get; set; }

        public const string IncludeTimeProperty = "it";
        [BsonElement(IncludeTimeProperty)]
        [BsonIgnoreIfNull(true)]
        public bool IncludeTime { get; set; }

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

        [BsonElement(ExProperty)]
        public string Ex { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        public Status Status { get; set; }

        public const string ResponsesProperty = "resp";
        [BsonElement(ResponsesProperty)]
        public List<MEResponse> Responses { get; set; }
    }
}
