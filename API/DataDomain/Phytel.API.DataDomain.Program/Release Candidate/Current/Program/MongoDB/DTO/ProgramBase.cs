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
    public class ProgramBase : PlanElement
    {
        public ProgramBase() {}

        public const string EndDateProperty = "ed";
        [BsonElement(EndDateProperty)]
        [BsonIgnoreIfNull(false)]
        public DateTime? EndDate { get; set; }

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        public const string ShortNameProperty = "sn";
        [BsonElement(ShortNameProperty)]
        [BsonIgnoreIfNull(true)]
        public string ShortName { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        public const string ClientProperty = "cli";
        [BsonElement(ClientProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? Client { get; set; }

        public const string StatusProperty = "sts";
        [BsonElement(StatusProperty)]
        [BsonIgnoreIfNull(true)]
        public Status Status { get; set; }

        public const string ObjectivesInfoProperty = "obj";
        [BsonElement(ObjectivesInfoProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Objective> Objectives { get; set; }

        public const string ModulesProperty = "ms";
        [BsonElement(ModulesProperty)]
        [BsonIgnoreIfNull(true)]
        public List<Module> Modules { get; set; }

        #region // will be refactored
        //public const string EligibilityRequirementsProperty = "er";
        //[BsonElement(EligibilityRequirementsProperty)]
        //[BsonIgnoreIfNull(true)]
        //public string EligibilityRequirements { get; set; }

        //public const string EligibilityStartDateProperty = "esd";
        //[BsonElement(EligibilityStartDateProperty)]
        //[BsonIgnoreIfNull(true)]
        //public DateTime? EligibilityStartDate { get; set; }

        //public const string EligibilityEndDateProperty = "eedt";
        //[BsonElement(EligibilityEndDateProperty)]
        //[BsonIgnoreIfNull(false)]
        //public DateTime? EligibilityEndDate { get; set; }

        //public const string AuthoredByProperty = "athby";
        //[BsonElement(AuthoredByProperty)]
        //[BsonIgnoreIfNull(true)]
        //public string AuthoredBy { get; set; }

        //public const string LockedProperty = "lck";
        //[BsonElement(LockedProperty)]
        //[BsonIgnoreIfNull(true)]
        //public bool Locked { get; set; }
        #endregion
    }
}
