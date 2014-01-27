using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    public class MESpawnElement
    {
        public MESpawnElement() { }

        public const string TypeProperty = "type";
        [BsonElement(TypeProperty)]
        [BsonIgnoreIfNull(true)]
        public int Type { get; set; }

        public const string SpawnIdProperty = "spwnid";
        [BsonElement(SpawnIdProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId SpawnId { get; set; }

        public const string TagProperty = "tag";
        [BsonElement(TagProperty)]
        [BsonIgnoreIfNull(true)]
        public string Tag { get; set; }
    }
}
