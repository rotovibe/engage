using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class LookUpDetailsBase : LookUpBase
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
