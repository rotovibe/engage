using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class LookUpBase : ILookUpData
    {
        public const string DataIdProperty = "did";
        public const string NameProperty = "nm";

        [BsonElement(DataIdProperty)]
        public ObjectId DataId { get; set; }

        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }
    }
}
