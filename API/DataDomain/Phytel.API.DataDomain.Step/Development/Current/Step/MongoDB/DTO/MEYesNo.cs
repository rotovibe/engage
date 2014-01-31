using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class MEYesNo : MEStepBase
    {
        //public const string QuestionProperty = "q";
        public const string NotesProperty = "n";

        [BsonElement(QuestionProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string Question { get; set; }

        [BsonElement(NotesProperty)]
        [BsonIgnoreIfNull(true)]
        public string Notes { get; set; }
    }
}
