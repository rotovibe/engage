using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class PatientObservationData
    {
        public string Id { get; set; }
        public string ObservationId { get; set; }
        public string PatientId { get; set; }
        public string Name { get; set; }
        public List<ObservationValueData> Values { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Units { get; set; }
        public bool Standard { get; set; }
        public int? Order { get; set; }
        public string TypeId { get; set; }
        public string GroupId { get; set; }
        public string Source { get; set; }
        public int StateId { get; set; }
        public int DisplayId { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }
    }
}
