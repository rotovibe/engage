using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class CommType : LookUpBase
    {
        public const string CommModesProperty = "cmode";

        [BsonElement(CommModesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> CommModes { get; set; }
    }
}
