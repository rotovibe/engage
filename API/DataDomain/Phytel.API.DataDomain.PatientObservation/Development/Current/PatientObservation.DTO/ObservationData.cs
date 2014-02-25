using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class ObservationData
    {
        public string Id { get; set; }
        public string ObservationType { get; set; }
        public string GroupId { get; set; }
        public string Description { get; set; }
        public string CommonName { get; set; }
        public bool Standard { get; set; }
        public int? Order { get; set; }
        public string CodingSystem { get; set; }
        public string CodingSystemCode { get; set; }
        public int? LowValue { get; set; }
        public int? HighValue { get; set; }
        public int Status { get; set; }
        public string Units { get; set; }
        public string Source { get; set; }
        public Dictionary<string, object> ExtraElements { get; set; }
        public string Version { get; set; }
        public string UpdatedBy { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? TTLDate { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
