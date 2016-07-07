using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class RefusalReason : LookUpDetailsBase
    {
        public const string CodingSystemIdProperty = "csid";
        public const string CodingSystemCodeProperty = "csc";
        public const string TypeProperty = "type";

        [BsonElement(CodingSystemIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? CodingSystemId { get; set; }

        [BsonElement(CodingSystemCodeProperty)]
        [BsonIgnoreIfNull(true)]
        public string CodingSystemCode { get; set; }

        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public string Type { get; set; }
    }
}