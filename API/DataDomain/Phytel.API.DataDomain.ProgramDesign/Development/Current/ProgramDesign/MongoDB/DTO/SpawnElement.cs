using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;
using Phytel.API.DataDomain.ProgramDesign;

namespace Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO
{
    public class SpawnElement
    {
        public SpawnElement() { }

        public const string TypeProperty = "type";
        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public SpawnElementTypeCode Type { get; set; }

        public const string SpawnIdProperty = "spwnid";
        [BsonElement(SpawnIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? SpawnId { get; set; }

        public const string TagProperty = "tag";
        [BsonElement(TagProperty)]
        [BsonIgnoreIfNull(true)]
        public string Tag { get; set; }
    }
}
