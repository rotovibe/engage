using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class METimeZone : LookUpBase
    {
        public const string DefaultProperty = "df";

        [BsonElement(DefaultProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Default { get; set; }
    }
}
