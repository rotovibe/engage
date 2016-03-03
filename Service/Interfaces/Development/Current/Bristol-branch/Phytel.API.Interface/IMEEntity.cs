using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Phytel.API.Interface
{
    public interface IMEEntity
    {
        [BsonExtraElements]
        BsonDocument ExtraElements { get; set; }

        [BsonElement("v")]
        [BsonDefaultValue(1.0)]
        double Version { get; set; }

        [BsonElement( "uby")]
        [BsonIgnoreIfNull(true)]
        ObjectId? UpdatedBy { get; set; }

        [BsonElement("del")]
        [BsonDefaultValue(false)]
        bool DeleteFlag { get; set; }

        [BsonElement("ttl")]
        [BsonDefaultValue(null)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        DateTime? TTLDate { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement("uon")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        DateTime? LastUpdatedOn { get; set; }

        [BsonIgnoreIfNull(true)]
        [BsonElement("rcby")]
        ObjectId RecordCreatedBy { get; }

        [BsonIgnoreIfNull(true)]
        [BsonElement("rcon")]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
        DateTime RecordCreatedOn { get; }
    }
}
