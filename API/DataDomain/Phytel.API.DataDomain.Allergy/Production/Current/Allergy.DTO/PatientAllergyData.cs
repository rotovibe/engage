using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class PatientAllergyData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string AllergyId { get; set; }
        public string AllergyName { get; set; }
        public List<string> AllergyTypeIds { get; set; }
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
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }
        public string CodingSystemId { get; set; }
        public string Code { get; set; }
    }
}
