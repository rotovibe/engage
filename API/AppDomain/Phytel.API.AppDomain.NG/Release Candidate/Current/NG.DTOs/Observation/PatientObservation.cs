using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.AppDomain.NG.DTO.Observation
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
    }
}
