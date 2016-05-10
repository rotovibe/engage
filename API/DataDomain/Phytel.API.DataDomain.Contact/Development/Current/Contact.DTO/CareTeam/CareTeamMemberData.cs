using System;

namespace Phytel.API.DataDomain.Contact.DTO.CareTeam
{
    public class CareTeamMemberData 
    {
        public string Id { get; set; }
        public string ContactId { get; set; }
        public string RoleId { get; set; }
        public string CustomRoleName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Core { get; set; }
        public string Notes { get; set; }
        public string FrequencyId { get; set; }
        public int? Distance { get; set; }
        public string DistanceUnit { get; set; }
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }        
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedById { get; set; }
        public string CreatedById { get; set; }
        public int StatusId { get; set; }
    }
}
