using System;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class MedicationMapData
    {
        public string Id { get; set; }
        public string SubstanceName { get; set; }
        public string FullName { get; set; }
        public string Strength { get; set; }
        public string Route { get; set; }
        public string Form { get; set; }
        public bool Custom { get; set; }
        public bool Verified { get; set; }

        #region Base elements

        public double Version { get; set; }
        public string UpdatedBy { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime? TTLDate { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string RecordCreatedBy { get; set; }
        public DateTime RecordCreatedOn { get; set; }

        #endregion
    }
}