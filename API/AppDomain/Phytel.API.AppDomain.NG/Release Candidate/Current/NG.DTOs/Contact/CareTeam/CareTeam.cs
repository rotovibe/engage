using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class CareTeam
    {
        public string Id { get; set; }
        public string ContactId { get; set; }
        public List<Member> Members { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedById { get; set; }
        public string CreatedById { get; set; }
    }
}
