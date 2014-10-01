using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.Mongo.Linq;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.Security
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { ApiKeyProperty, ProductProperty, IsActiveProperty })]
    public class MEAPIUser : IMongoEntity<ObjectId>
    {
        public MEAPIUser() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";

        public const string UserNameProperty = "un";
        public const string PasswordProperty = "pwd";
        public const string SaltProperty = "salt";
        public const string ApiKeyProperty = "apikey";
        public const string ProductProperty = "product";
        public const string IsActiveProperty = "isactive";
        public const string SessionLengthInMinutesProperty = "slim";
        public const string VersionProperty = "v";
        public const string DefaultContractProperty = "dc";

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement(UserNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string UserName { get; set; }

        [BsonElement(PasswordProperty)]
        [BsonIgnoreIfNull(true)]
        public string Password { get; set; }

        [BsonElement(SaltProperty)]
        [BsonIgnoreIfNull(true)]
        public string Salt { get; set; }

        [BsonElement(ApiKeyProperty)]
        [BsonIgnoreIfNull(true)]
        public string ApiKey { get; set; }

        [BsonElement(DefaultContractProperty)]
        [BsonIgnoreIfNull(true)]
        public string DefaultContract { get; set; }
        
        [BsonElement(ProductProperty)]
        [BsonIgnoreIfNull(true)]
        public string Product { get; set; }

        [BsonElement(IsActiveProperty)]
        [BsonIgnoreIfNull(true)]
        public bool IsActive { get; set; }

        [BsonElement(SessionLengthInMinutesProperty)]
        [BsonDefaultValue(60)]
        public int SessionLengthInMinutes { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue(1.0)]
        public double Version { get; set; }

        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }
    }
}
