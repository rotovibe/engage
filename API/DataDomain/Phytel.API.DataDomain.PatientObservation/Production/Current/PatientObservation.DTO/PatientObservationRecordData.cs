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
        public double? Value { get; set; }
        public string NonNumericValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Units { get; set; }
        public string TypeId { get; set; }
        public string GroupId { get; set; }
        public string Source { get; set; }
        public string SetId { get; set; }
        public int StateId { get; set; }
        public int DisplayId { get; set; }
        public bool DeleteFlag { get; set; }
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }
    }
}
