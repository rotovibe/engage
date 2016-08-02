using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class MESelect : MEStepBase
    {
        public const string InstructionsProperty = "inst";
        [BsonElement(InstructionsProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string Instructions { get; set; }

        [BsonElement(QuestionProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string Question { get; set; }

        public const string ResponseProperty = "resp";
        [BsonElement(ResponseProperty)]
        [BsonIgnoreIfNull(true)]
        public List<MEResponse> Response { get; set; }

        public const string SelectTypeProperty = "selt";
        [BsonElement(SelectTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public SelectType SelectType { get; set; }

        public const string ControlTypeProperty = "ctype";
        [BsonElement(ControlTypeProperty)]
        [BsonIgnoreIfNull(true)]
        public ControlType ControlType { get; set; }
    }

    public enum SelectType
    {
        Single = 1,
        Multi = 2
    }

    public enum ControlType
    {
        CheckBox = 1,
        List = 2,
        Radio = 3
    }
}
