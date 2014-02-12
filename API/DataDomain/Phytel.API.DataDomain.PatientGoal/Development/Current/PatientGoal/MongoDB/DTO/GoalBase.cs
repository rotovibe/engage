using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Mongo.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GoalBase
    {
        public GoalBase() {}

        public const string NameProperty = "nm";
        [BsonElement(NameProperty)]
        [BsonIgnoreIfNull(true)]
        public string Name { get; set; }

        public const string DescriptionProperty = "desc";
        [BsonElement(DescriptionProperty)]
        [BsonIgnoreIfNull(true)]
        public string Description { get; set; }

        public const string StatusDateProperty = "stsd";
        [BsonElement(StatusDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]
        public DateTime? StatusDate { get; set; }

        public const string StartDateProperty = "sd";
        [BsonElement(StartDateProperty)]
        [BsonIgnoreIfNull(true)]
        [BsonDateTimeOptions(Kind = System.DateTimeKind.Local)]        
        public DateTime? StartDate { get; set; }
    }
}
