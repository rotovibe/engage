using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientObservation
    {
        public string Id { get; set; }
        public string ObservationId { get; set; }
        public string PatientId { get; set; }
        public string Name { get; set; }
        public List<ObservationValue> Values { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Units { get; set; }
        public bool Standard { get; set; }
        public string TypeId { get; set; }
        public string GroupId { get; set; }
        public int Order { get; set; }
        public string Source { get; set; }
        public int StateId { get; set; }
        public int DisplayId { get; set; }
        public bool DeleteFlag { get; set; }
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }
    }
}
