using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class Objective : LookUpBase
    {
        public const string DescriptionProperty = "desc";
        public const string CategoriesProperty = "cats";

        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        [BsonElement(CategoriesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<ObjectId> CategoryIds { get; set; }
    }
}
