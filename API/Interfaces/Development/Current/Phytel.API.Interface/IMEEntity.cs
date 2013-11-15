using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.Interface
{
    public interface IMEEntity
    {
        [BsonExtraElements]
        [BsonIgnoreIfNull(true)]
        [BsonElement("ex")]
        Dictionary<string, object> ExtraElements { get; set; }

        [BsonElement("v")]
        [BsonIgnoreIfNull(true)]
        string Version { get; set; }

        [BsonElement( "uby")]
        [BsonIgnoreIfNull(true)]
        string UpdatedBy { get; set; }

        [BsonElement("del")]
        [BsonDefaultValue(0)]
        [BsonIgnoreIfNull(true)]
        bool DeleteFlag { get; set; }

        [BsonElement("ttl")]
        [BsonDefaultValue(null)]
        [BsonIgnoreIfNull(true)]
        DateTime TTLDate { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement("uon")]
        DateTime LastUpdatedOn { get; set; }
    }
}
