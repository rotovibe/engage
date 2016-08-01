using System;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Member
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
        public double? Distance { get; set; }
        public string DistanceUnit { get; set; }
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }
        public int StatusId { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedById { get; set; }
        public string CreatedById { get; set; }
        // Contact is the whole object graph of "ContactId". Populated only in GET calls. Contact object will be ignored in POST & PUT calls if used.
        public Contact Contact { get; set; }
    }
}
