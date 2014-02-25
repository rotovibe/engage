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
        public string Name { get; set; }
        public List<ObservationValueData> Values { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Standard { get; set; }
        public int? Order { get; set; }
        public int Type { get; set; }
        public string GroupId { get; set; }
    }
}
