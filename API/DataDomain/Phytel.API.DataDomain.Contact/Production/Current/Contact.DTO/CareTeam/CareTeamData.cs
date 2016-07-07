using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Contact.DTO.CareTeam
{
    public class CareTeamData
    {
        public string Id { get; set; }
        public string ContactId { get; set; }
        public List<CareTeamMemberData> Members { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedById { get; set; }
        public string CreatedById { get; set; }
    }
}
