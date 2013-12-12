using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class MEObjective : LookUpBase
    {
        public const string DescriptionProperty = "desc";
        public const string CategoriesProperty = "cats";

        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        [BsonElement(CategoriesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<CategoryInfo> Categories { get; set; }
    }

    public class CategoryInfo
    { 
        public ObjectId Id { get; set; }
        public string Text { get; set; }
    }
}
