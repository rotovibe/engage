using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Step.DTO
{
    [BsonIgnoreExtraElements(false)]
    public class MEYesNo : MEStepBase
    {
        //public const string QuestionProperty = "q";

        [BsonElement(QuestionProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonRequired]
        public string Question { get; set; }

        public const string NotesProperty = "nts";
        [BsonElement(NotesProperty)]
        [BsonIgnoreIfNull(true)]
        public string Notes { get; set; }
    }
}
