using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppDomain.Engage.Clinical.DTO.Medications
{
    public class MedicationData
    {
        // add poco properties
        public string contract { get; set; }
        public string externalSystem { get; set; }
        public string externalRecordId { get; set; }
        public string patientId { get; set; }
        public string sourceType { get; set; }
        public string category { get; set; }
        public string medName { get; set; }
        public string[] medCodes { get; set; }
        public string[] medClasses { get; set; }
        public string medType { get; set; }
        public string reason { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string strength { get; set; }
        public string sig { get; set; }
        public string route { get; set; }
        public string form { get; set; }
        public string duration { get; set; }
        public string dosage { get; set; }
        public string dosageFreq { get; set; }
        public string notes { get; set; }
        public string prescribedBy { get; set; }

    }
}
