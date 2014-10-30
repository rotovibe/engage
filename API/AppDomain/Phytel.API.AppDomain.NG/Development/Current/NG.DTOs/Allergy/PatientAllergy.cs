using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientAllergy
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string AllergyId { get; set; }
        public string AllergyName { get; set; }
        public List<string> AllergyTypeId { get; set; }
        public string SeverityId { get; set; }
        public List<string> ReactionIds { get; set; }
        public string SourceId { get; set; }
        public string Notes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StatusId { get; set; }
        public string SystemName { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
