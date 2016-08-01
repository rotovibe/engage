using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Phytel.API.Interface;
using Phytel.Services.Mongo.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using Phytel.API.Common;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    public class PlanElement
    {
        public PlanElement() { }

        // This is an individual attribute
        public const string AttributeEndDateProperty = "attred";
        [BsonElement(AttributeEndDateProperty)]
        [BsonIgnoreIfNull(false)]
        public DateTime? AttributeEndDate { get; set; }

        // This is an individual attribute
        public const string AttributeStartDateProperty = "attrsd";
        [BsonElement(AttributeStartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? AttributeStartDate { get; set; }

        public const string ArchiveOriginIdProperty = "archoid";
        [BsonElement(ArchiveOriginIdProperty)]
        [BsonIgnoreIfNull(true)]
        public string ArchiveOriginId { get; set; }

        public const string ArchiveIdProperty = "archid";
        [BsonElement(ArchiveIdProperty)]
        [BsonIgnoreIfNull(true)]
        public bool Archived { get; set; }

        public const string ArchiveDateProperty = "archdate";
        [BsonElement(ArchiveDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? ArchivedDate { get; set; }

        public const string SourceIdProperty = "srcid";
        [BsonElement(SourceIdProperty)]
        [BsonIgnoreIfNull(false)]
        public ObjectId SourceId { get; set; }

        public const string OrderProperty = "o";
        [BsonElement(OrderProperty)]
        [BsonIgnoreIfNull(false)]
        public int Order { get; set; }

        public const string EnabledProperty = "e";
        [BsonElement(EnabledProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Enabled { get; set; }

        public const string StateProperty = "st";
        [BsonElement(StateProperty)]
        [BsonIgnoreIfNull(true)]
        public ElementState State { get; set; }

        public const string StateUpdatedOnProperty = "stuon";
        [BsonElement(StateUpdatedOnProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? StateUpdatedOn { get; set; }

        public const string AssignDateProperty = "aon";
        [BsonElement(AssignDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? AssignedOn { get; set; }

        public const string AssignByProperty = "aby";
        [BsonElement(AssignByProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? AssignedBy { get; set; }

        public const string AssignToProperty = "ato";
        [BsonElement(AssignToProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? AssignedTo { get; set; }

        public const string CompletedByProperty = "cby";
        [BsonElement(CompletedByProperty)]
        [BsonIgnoreIfNull(true)]
        public string CompletedBy { get; set; }

        public const string CompletedProperty = "cpld";
        [BsonElement(CompletedProperty)]
        [BsonIgnoreIfNull(false)]
        public bool Completed { get; set; }

        public const string CompletedOnProperty = "dc";
        [BsonElement(CompletedOnProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? DateCompleted { get; set; }

        public const string NextProperty = "nxt";
        [BsonElement(NextProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? Next { get; set; }

        public const string PreviousProperty = "prev";
        [BsonElement(PreviousProperty)]
        [BsonIgnoreIfNull(true)]
        public ObjectId? Previous { get; set; }

        public const string SpawnProperty = "spwn";
        [BsonElement(SpawnProperty)]
        [BsonIgnoreIfNull(true)]
        public List<SpawnElement> Spawn { get; set; }

        public const string EligibilityRequirementsProperty = "er";
        [BsonElement(EligibilityRequirementsProperty)]
        [BsonIgnoreIfNull(true)]
        public string EligibilityRequirements { get; set; }

        public const string EligibilityStartDateProperty = "esd";
        [BsonElement(EligibilityStartDateProperty)]
        [BsonIgnoreIfNull(true)]
        public DateTime? EligibilityStartDate { get; set; }

        public const string EligibilityEndDateProperty = "eedt";
        [BsonElement(EligibilityEndDateProperty)]
        [BsonIgnoreIfNull(false)]
        public DateTime? EligibilityEndDate { get; set; }
    }
}