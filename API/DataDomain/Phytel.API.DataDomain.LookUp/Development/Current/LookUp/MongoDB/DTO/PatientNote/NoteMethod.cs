using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class NoteMethod : LookUpBase
    {
        public const string DefaultProperty = "df";
        public const string ActiveProperty = "act";

        [BsonElement(DefaultProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Default { get; set; }

        [BsonElement(ActiveProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Active { get; set; }
    }
}
