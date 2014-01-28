using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class MELanguage : LookUpBase
    {
        public const string CodeProperty = "c";
        public const string ActiveProperty = "act";

        [BsonElement(CodeProperty)]
        [BsonIgnoreIfNull(true)]
        public string Code { get; set; }

        [BsonElement(ActiveProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Active { get; set; }
    }
}
