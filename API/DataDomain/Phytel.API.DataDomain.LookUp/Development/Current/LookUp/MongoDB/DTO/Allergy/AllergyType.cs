using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class AllergyType : LookUpBase
    {
        public const string CodingSystemIdProperty = "csid";
        public const string CodingSystemCodeProperty = "csc";

        [BsonElement(CodingSystemIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? CodingSystemId { get; set; }

        [BsonElement(CodingSystemCodeProperty)]
        [BsonIgnoreIfNull(true)]
        public string CodingSystemCode { get; set; }
    }
}