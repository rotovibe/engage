using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class PatientObservationRecordData
    {
        public string Id { get; set; }
        public float Value { get; set; }
        public string NonNumericValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Units { get; set; }
        public string TypeId { get; set; }
        public string GroupId { get; set; }
    }
}
