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

        //Addded for Patient Import Tool
        public string CMan { get; set; }
        public string sysName { get; set; }
        public string SysPri { get; set; }
        public string ActivateDeactivate { get; set; }
        public string SysId { get; set; }
        public string TimeZ { get; set; }
        public string Ph1 { get; set; }
        public string Ph1Pref { get; set; }
        public string Ph1Type { get; set; }
        public string Ph2 { get; set; }
        public string Ph2Pref { get; set; }
        public string Ph2Type { get; set; }
        public string Em1 { get; set; }
        public string Em1Pref { get; set; }
        public string Em1Type { get; set; }
        public string Em2  { get; set; }
        public string Em2Pref { get; set; }
        public string Em2Type { get; set; }
        public string Add1L1 { get; set; }
        public string Add1L2 { get; set; }
        public string Add1L3 { get; set; }
        public string Add1City { get; set; }
        public string Add1St { get; set; }
        public string Add1Zip { get; set; }
        public string Add1Pref { get; set; }
        public string Add1Type { get; set; }
        public string Add2L1 { get; set; }
        public string Add2L2 { get; set; }
        public string Add2L3 { get; set; }
        public string Add2City { get; set; }
        public string Add2St { get; set; }
        public string Add2Zip { get; set; }
        public string Add2Pref { get; set; }
        public string Add2Type { get; set; }

    }
}