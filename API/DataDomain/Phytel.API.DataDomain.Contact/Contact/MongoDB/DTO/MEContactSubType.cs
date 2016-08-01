using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class MEContactSubType
    {

        public MEContactSubType()
        {
            Id = ObjectId.GenerateNewId();
        }
        
        public const string IdProperty = "_id";
        [BsonElement(IdProperty)]
        public ObjectId Id { get; set; }

        public const string SubTypeIdProperty = "subtypeid";
        [BsonElement(SubTypeIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SubTypeId { get; set; }

        public const string SpecialtyIdProperty = "spid";
        [BsonElement(SpecialtyIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SpecialtyId { get; set; }

        public const string SubSpecialtyIdProperty = "subspid";
        [BsonElement(SubSpecialtyIdProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> SubSpecialtyIds { get; set; }
    }
}