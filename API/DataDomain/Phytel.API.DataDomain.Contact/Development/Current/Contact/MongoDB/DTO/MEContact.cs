using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [BsonIgnoreExtraElements(false)]
    [MongoIndex(Keys = new string[] { TTLDateProperty }, TimeToLive = 0)]
    public class MEContact : IMongoEntity<ObjectId>, IMEEntity
    {
        public MEContact() { Id = ObjectId.GenerateNewId(); }

        public const string IdProperty = "_id";

        #region Standard IMongoEntity Constants
        public const string ExtraElementsProperty = "ex";
        public const string VersionProperty = "v";
        public const string UpdatedByProperty = "uby";
        public const string DeleteFlagProperty = "del";
        public const string TTLDateProperty = "ttl";
        public const string LastUpdatedOnProperty = "uon";
        #endregion

        [BsonId]
        public ObjectId Id { get; set; }


        #region Standard IMongoEntity Implementation
        [BsonElement(ExtraElementsProperty)]
        [BsonExtraElements()]
        [BsonIgnoreIfNull(true)]
        public Dictionary<string, object> ExtraElements { get; set; }

        [BsonElement(VersionProperty)]
        [BsonDefaultValue("v1")]
        public string Version { get; set; }

        [BsonElement(UpdatedByProperty)]
        [BsonDefaultValue("-100")]
        public string UpdatedBy { get; set; }

        [BsonElement(DeleteFlagProperty)]
        [BsonDefaultValue(false)]
        public bool DeleteFlag { get; set; }

        [BsonElement(TTLDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? TTLDate { get; set; }

        [BsonElement(LastUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public System.DateTime? LastUpdatedOn { get; set; }
        #endregion

        //public string ContactId { get; set; }
        //public string PatientId { get; set; }
        //public string UserId { get; set; }
        //public List<CommMode> Modes { get; set; }
        //public List<Phone> Phones { get; set; }
        //public List<Text> Texts { get; set; }
        //public List<Email> Emails { get; set; }
        //public List<Address> Addresses { get; set; }
        //public List<int> WeekDays { get; set; }
        //public List<string> TimesOfDaysId { get; set; }
        //public string TimeZoneId { get; set; }
        //public List<Language> Languages { get; set; }

        //public string PreferredPhoneId { get; set; }
        //public string PreferredTextId { get; set; }
        //public string PreferredEmailId { get; set; }
        //public string PreferredAddressId { get; set; }
        //public string PreferredLanguageId { get; set; }
    }
}
