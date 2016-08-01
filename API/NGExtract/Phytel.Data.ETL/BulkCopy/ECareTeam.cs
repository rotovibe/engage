using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Data.ETL.BulkCopy
{
    public class ECareTeam
    {
        public string MongoCareTeamId { get; set; }
        public string MongoContactIdForPatient { get; set; }
        public string MongoCareMemberId { get; set; }
        public string MongoContactIdForCareMember { get; set; }
        public string RoleId { get; set; }
        public string CustomRoleName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Core { get; set; }
        public string Notes { get; set; }
        public string FrequencyId { get; set; }
        public double? Distance { get; set; }
        public string DistanceUnit { get; set; }
        public string DataSource { get; set; }
        public string ExternalRecordId { get; set; }
        public string Status { get; set; }
        // Standard fields
        public double Version { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? TTLDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
    }
}
