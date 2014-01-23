using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class MECommType : LookUpBase
    {
        public const string CommModesProperty = "cmode";

        [BsonElement(CommModesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> CommModes { get; set; }
    }
}
