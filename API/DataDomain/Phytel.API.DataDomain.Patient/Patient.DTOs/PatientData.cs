using System;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Patient.DTO
{
    
    public class PatientData : IAppData
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string DataSource { get; set; }
        public string Gender { get; set; }
        public string ReasonId { get; set; }
        public int StatusId { get; set; }
        public string StatusDataSource { get; set; }
        public string DOB { get; set; }
        public double Version { get; set; }
        public string DisplayPatientSystemId { get; set; }
        public int PriorityData { get; set; }
        public bool Flagged { get; set; }
        public string Background { get; set; }
        public string ClinicalBackground { get; set; }
        public string LastFourSSN { get; set; }
        public string FullSSN { get; set; }
        public string MaritalStatusId { get; set; }
        public bool Protected { get; set; }
        public int DeceasedId { get; set; }
        // added for atmosphere integration
        public DateTime? LastUpdatedOn { get; set; }
        public DateTime RecordCreatedOn { get; set; }
        public string ExternalRecordId { get; set; }
        public string EngagePatientSystemValue { get; set; }
        public string UpdatedByProperty {get; set; }
        public string Prefix { get; set; }
    }
}