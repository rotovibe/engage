using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class MedicationData
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string NDC { get; set; }
        public string FullName { get; set; }
        public string ProprietaryName { get; set; }
        public string ProprietaryNameSuffix { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SubstanceName { get; set; }
        public List<string> PharmClass { get; set; }
        public string Route { get; set; }
        public string Form { get; set; }
        public string Unit { get; set; }
        public string Strength { get; set; }
        public string FamilyId { get; set; }

        public double Version { get; set; }
        public string UpdatedBy { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? TtlDate { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }
    }
}
